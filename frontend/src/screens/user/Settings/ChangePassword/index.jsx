import { useContext } from "react";
import { Formik } from "formik";
import isEmpty from "lodash/isEmpty";
import { useAlert } from "react-alert";

import { Button, ControlledInput, Text, Spinner } from "src/components";

import {
  buttonKinds,
  colorNames,
  textTypes,
  spinnerSizes,
  inputKinds,
} from "src/app-globals";

import { UserContext } from "src/contexts";
import { isValidPassword } from "src/utils/string";
import { useUpdateUser } from "src/hooks";

import styles from "./styles.module.scss";

const ChangePassword = () => {
  const alert = useAlert();
  const { user, loginUpdate } = useContext(UserContext);
  const {
    isUpdating: isUserUpdating,
    isVerifyingPassword,
    updateUser,
  } = useUpdateUser();

  const validate = (values) => {
    const errors = {};

    if (!values.oldPassword) {
      errors.oldPassword = "This field is required.";
    }

    if (!values.newPassword) {
      errors.newPassword = "This field is required.";
    } else if (!isValidPassword(values.newPassword)) {
      errors.newPassword =
        "Password must be at least 8 characters long and must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    }

    if (!values.confirmPassword) {
      errors.confirmPassword = "This field is required.";
    } else if (values.confirmPassword !== values.newPassword) {
      errors.confirmPassword = "This must match with your new password.";
    }

    return errors;
  };

  return (
    <div className={styles.ChangePassword}>
      <Text type={textTypes.HEADING.SM} className={styles.ChangePassword_title}>
        Change Password
      </Text>

      <Formik
        initialValues={{
          oldPassword: "",
          newPassword: "",
          confirmPassword: "",
        }}
        onSubmit={async (values, { setErrors, setFieldValue }) => {
          const currentFormValues = {
            firstName: user.firstName,
            lastName: user.lastName,
            email: user.email,
            username: user.username,
            password: values.newPassword,
          };

          const errors = validate(values);
          if (!isEmpty(errors)) {
            setErrors(errors);
            return;
          }

          const { responseCode: updateUserResponseCode, errors: updateErrors } =
            await updateUser(user.id, currentFormValues, values.oldPassword);

          const updateUserCallbacks = {
            updated: () => {
              alert.success("Account updated successfully.");

              loginUpdate({
                ...user,
                ...currentFormValues,
              });

              setFieldValue("oldPassword", "");
              setFieldValue("newPassword", "");
              setFieldValue("confirmPassword", "");
            },
            invalidFields: () => {
              alert.error("Invalid fields.");
              errors.oldPassword = updateErrors.password;
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
              className={styles.ChangePassword_input}
              kind={inputKinds.PASSWORD}
              placeholder="Old Password*"
              name="oldPassword"
              icon="pin"
              value={values.oldPassword}
              error={errors.oldPassword}
              onChange={(e) => setFieldValue("oldPassword", e.target.value)}
            />
            <ControlledInput
              className={styles.ChangePassword_input}
              kind={inputKinds.PASSWORD}
              placeholder="New Password*"
              name="newPassword"
              icon="lock"
              value={values.newPassword}
              error={errors.newPassword}
              onChange={(e) => setFieldValue("newPassword", e.target.value)}
            />

            <ControlledInput
              className={styles.ChangePassword_input}
              kind={inputKinds.PASSWORD}
              placeholder="Confirm Password*"
              name="confirmPassword"
              icon="vpn_key"
              value={values.confirmPassword}
              error={errors.confirmPassword}
              onChange={(e) => setFieldValue("confirmPassword", e.target.value)}
            />

            <div className={styles.ChangePassword_buttonGroup}>
              <Button
                className={styles.ChangePassword_buttonGroup_updateButton}
                kind={buttonKinds.SUBMIT}
                disabled={isUserUpdating}
                onClick={() => {}}
              >
                <span className={styles.ChangePassword_buttonGroup_buttonText}>
                  Update
                  {isVerifyingPassword && isUserUpdating && (
                    <Spinner
                      size={spinnerSizes.XS}
                      colorName={colorNames.WHITE}
                      className={styles.ChangePassword_buttonGroup_spinner}
                    />
                  )}
                </span>
              </Button>
            </div>
          </form>
        )}
      </Formik>
    </div>
  );
};

export default ChangePassword;
