import { useRef, useState } from "react";
import PropTypes from "prop-types";
import Cookies from "universal-cookie";
import isEmpty from "lodash/isEmpty";
import { Formik } from "formik";

import styles from "../styles.module.scss";
import { Button, ControlledInput, Modal } from "src/components";
import {
  buttonTypes,
  inputTypes,
  modalPositions,
  modalSizes,
} from "src/app-globals";

import { ConfirmationCodesService } from "src/services";
import { useInterval } from "src/hooks";

const RESEND_VERIFICATION_COOLDOWN = 30;

const validate = (values) => {
  const errors = {};

  if (!values.code) {
    errors.code = "This field is required.";
  }

  return errors;
};

const InputConfirmationCodeModal = ({
  isOpen,
  handleClose,
  handleSuccess,
  user,
}) => {
  const formRef = useRef(null);
  const cookies = new Cookies();
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [resendCooldown, setResendCooldown] = useState(0);

  const resendVerification = () => {
    setResendCooldown(RESEND_VERIFICATION_COOLDOWN);

    ConfirmationCodesService.send(user.email);
  };

  useInterval(() => {
    if (resendCooldown === 0) {
      return;
    }

    setResendCooldown(resendCooldown - 1);
  }, 1000);

  return (
    <Modal
      size={modalSizes.SM}
      position={modalPositions.CENTER}
      isOpen={isOpen}
      handleClose={handleClose}
      title="Confirm your Email"
      className={styles.ForgotPasswordModals}
      hasCloseButton={false}
      actions={[
        {
          text: "Submit",
          type: buttonTypes.PRIMARY.GREEN,
          disabled: isSubmitting === true,
          onClick: () => {
            formRef.current.handleSubmit();
          },
        },
      ]}
    >
      <Formik
        innerRef={formRef}
        initialValues={{ code: "" }}
        onSubmit={async (values, { setErrors }) => {
          const errors = validate(values);
          if (!isEmpty(errors)) {
            setErrors(errors);
            return;
          }

          setIsSubmitting(true);

          try {
            // Verify the confirmation code
            const {
              data: verifyConfirmationCodeResponse,
              status: verifyConfirmationCodeStatus,
            } = await ConfirmationCodesService.verify({
              email: user.email,
              code: values.code,
            });

            if (verifyConfirmationCodeStatus === 200) {
              // If the request was successful, set the accessToken cookie to access the update endpoint
              cookies.set("accessToken", verifyConfirmationCodeResponse, {
                path: "/",
              });

              // Proceed to the next modal
              handleSuccess();
            }

            setIsSubmitting(false);
          } catch (error) {
            const status = error.response.status;

            switch (status) {
              case 400:
                setIsSubmitting(false);
                setErrors({
                  code: "The code you inputted is an invalid code.",
                });
                break;

              case 500:
                setIsSubmitting(false);
                setErrors({
                  code: "Oops, something went wrong.",
                });
                break;
              default:
                break;
            }
          }
        }}
      >
        {({ errors, values, handleSubmit, setFieldValue }) => (
          <form onSubmit={handleSubmit}>
            <ControlledInput
              className={styles.ForgotPasswordModals_withMarginBottom}
              type={inputTypes.SLIM}
              label="We've sent a confirmation code to your email. Input the code to proceed."
              placeholder="Confirmation Code*"
              name="code"
              id="code"
              value={values.code}
              error={errors.code}
              onChange={(e) =>
                setFieldValue("code", e.target.value.toUpperCase())
              }
            />
          </form>
        )}
      </Formik>
      <Button
        type={buttonTypes.TEXT.GREEN}
        onClick={resendVerification}
        disabled={resendCooldown > 0}
      >
        {resendCooldown > 0
          ? `Resend Verification (${resendCooldown}s)`
          : "Resend Verification"}
      </Button>
    </Modal>
  );
};

InputConfirmationCodeModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  handleClose: PropTypes.func.isRequired,
  user: PropTypes.object.isRequired,
  // doesn't accept any parameter. This will just
  // proceed to the next modal
  handleSuccess: PropTypes.func.isRequired,
};

export default InputConfirmationCodeModal;
