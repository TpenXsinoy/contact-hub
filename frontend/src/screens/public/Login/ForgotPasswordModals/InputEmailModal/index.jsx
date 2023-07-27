import { useRef, useState } from "react";
import PropTypes from "prop-types";
import { Formik } from "formik";
import isEmpty from "lodash/isEmpty";

import {
  modalSizes,
  modalPositions,
  buttonTypes,
  inputTypes,
} from "src/app-globals";
import { Modal, ControlledInput } from "src/components";
import { ConfirmationCodesService } from "src/services";

import styles from "../styles.module.scss";

const validate = (values) => {
  const errors = {};

  if (!values.emailAddress) {
    errors.emailAddress = "This field is required.";
  }

  return errors;
};

const InputEmailModal = ({ isOpen, handleClose, handleSuccess }) => {
  const formRef = useRef(null);
  const [isVerifying, toggleIsVerifying] = useState(false);

  return (
    <Modal
      size={modalSizes.SM}
      position={modalPositions.CENTER}
      isOpen={isOpen}
      handleClose={handleClose}
      title="Just Checking"
      className={styles.ForgotPasswordModals}
      actions={[
        {
          text: isVerifying ? "Loading..." : "Proceed",
          type: buttonTypes.PRIMARY.GREEN,
          disabled: isVerifying === true,
          onClick: () => {
            formRef.current.handleSubmit();
          },
        },
      ]}
    >
      <Formik
        innerRef={formRef}
        initialValues={{ login: "" }}
        onSubmit={async (values, { setErrors }) => {
          const errors = validate(values);
          if (!isEmpty(errors)) {
            setErrors(errors);
            return;
          }

          toggleIsVerifying(true);

          try {
            // Send confirmation code to user's email address
            // This API will also verify if the user exists
            const {
              data: sendConfirmationCodeResponse,
              status: sendConfirmationCodeResponseStatus,
            } = await ConfirmationCodesService.send(values.emailAddress);

            if (sendConfirmationCodeResponseStatus === 200) {
              handleSuccess({
                user: sendConfirmationCodeResponse,
              });
            }

            toggleIsVerifying(false);
          } catch (error) {
            const status = error.response.status;

            switch (status) {
              case 404:
                toggleIsVerifying(false);
                setErrors({
                  emailAddress: "The user does not exist.",
                });
                break;

              case 500:
                toggleIsVerifying(false);
                setErrors({
                  emailAddress: "Oops, something went wrong.",
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
              type={inputTypes.SLIM}
              label="For security purposes, we need your email address"
              placeholder="Email Address"
              name="emailAddress"
              value={values.emailAddress}
              error={errors.emailAddress}
              onChange={(e) => setFieldValue("emailAddress", e.target.value)}
            />
          </form>
        )}
      </Formik>
    </Modal>
  );
};

InputEmailModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  handleClose: PropTypes.func.isRequired,
  handleSuccess: PropTypes.func.isRequired,
};

export default InputEmailModal;
