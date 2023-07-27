import { useRef } from "react";
import PropTypes from "prop-types";
import { Formik } from "formik";
import isEmpty from "lodash/isEmpty";
import Cookies from "universal-cookie";

import { Modal, ControlledInput } from "src/components";
import {
  modalSizes,
  modalPositions,
  buttonTypes,
  inputTypes,
  inputKinds,
} from "src/app-globals";
import { useUpdateUser } from "src/hooks";
import { isValidPassword } from "src/utils/string";

import styles from "../styles.module.scss";

const validate = (values) => {
  const errors = {};

  if (!values.password) {
    errors.password = "This field is required.";
  } else if (!isValidPassword(values.password)) {
    errors.password =
      "Password must be at least 8 characters long and must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
  }

  if (!values.confirmPassword) {
    errors.confirmPassword = "This field is required.";
  } else if (values.password && values.password !== values.confirmPassword) {
    errors.confirmPassword = "This must match with your new password.";
  }

  return errors;
};

const ChangePasswordModal = ({ isOpen, handleClose, user, handleSuccess }) => {
  const formRef = useRef(null);
  const cookies = new Cookies();
  const {
    isUpdating: isUserUpdating,
    isVerifyingPassword,
    updateUser,
  } = useUpdateUser();

  return (
    <Modal
      size={modalSizes.SM}
      position={modalPositions.CENTER}
      isOpen={isOpen}
      handleClose={handleClose}
      title="New Beginnings"
      className={styles.ForgotPasswordModals}
      hasCloseButton={false}
      actions={[
        {
          text: "Proceed",
          type: buttonTypes.PRIMARY.GREEN,
          disabled: isUserUpdating || isVerifyingPassword,
          onClick: () => {
            formRef.current.handleSubmit();
          },
        },
      ]}
    >
      <Formik
        innerRef={formRef}
        initialValues={{ password: "", confirmPassword: "" }}
        onSubmit={async (values, { setErrors }) => {
          const errors = validate(values);
          if (!isEmpty(errors)) {
            setErrors(errors);
            return;
          }

          const userToBeUpdated = {
            firstName: user.firstName,
            lastName: user.lastName,
            email: user.email,
            username: user.username,
            password: values.password,
          };

          const { responseCode: updateUserResponseCode, errors: updateErrors } =
            await updateUser(user.id, userToBeUpdated, null, true);

          const updateUserCallbacks = {
            updated: () => {
              cookies.remove("accessToken", {
                path: "/",
              });
              handleSuccess();
            },
            invalidFields: () => {
              errors.password = updateErrors.password;
              setErrors(errors);
            },
            internalError: () => alert.error("Oops, something went wrong."),
          };

          switch (updateUserResponseCode) {
            case 200:
              updateUserCallbacks.updated();
              break;
            case 400:
              updateUserCallbacks.invalidFields();
              break;
            case 500:
              updateUserCallbacks.internalError();
              break;
            default:
              break;
          }
        }}
      >
        {({ errors, values, handleSubmit, setFieldValue }) => (
          <form onSubmit={handleSubmit}>
            <ControlledInput
              type={inputTypes.SLIM}
              label="For security purposes, please enter your new password below"
              kind={inputKinds.PASSWORD}
              className={styles.ForgotPasswordModals_withMarginBottom}
              placeholder="New Password"
              name="password"
              value={values.password}
              error={errors.password}
              onChange={(e) => setFieldValue("password", e.target.value)}
            />
            <ControlledInput
              type={inputTypes.SLIM}
              kind={inputKinds.PASSWORD}
              placeholder="Confirm Password"
              name="confirmPassword"
              value={values.confirmPassword}
              error={errors.confirmPassword}
              onChange={(e) => setFieldValue("confirmPassword", e.target.value)}
            />
          </form>
        )}
      </Formik>
    </Modal>
  );
};

ChangePasswordModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  handleClose: PropTypes.func.isRequired,
  user: PropTypes.object.isRequired,
  // function that doesn't accept any parameter. This just
  // redirects to the forgot password success modal
  handleSuccess: PropTypes.func.isRequired,
};

export default ChangePasswordModal;
