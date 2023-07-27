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
import { isValidEmail } from "src/utils/string";
import { useUpdateUser } from "src/hooks";

import styles from "./styles.module.scss";

const AccountInformation = () => {
  const alert = useAlert();

  const { user, loginUpdate } = useContext(UserContext);

  const {
    isUpdating: isUserUpdating,
    isVerifyingPassword,
    updateUser,
  } = useUpdateUser();

  const validate = (values) => {
    const errors = {};

    if (!values.firstName) {
      errors.firstName = "This field is required.";
    } else if (values.firstName.length > 50) {
      errors.firstName = "The maximum length of this field is 50 characters.";
    } else if (values.firstName.length < 2) {
      errors.firstName = "The minimum length of this field is two characters.";
    }

    if (!values.lastName) {
      errors.lastName = "This field is required.";
    } else if (values.lastName.length > 50) {
      errors.lastName = "The maximum length of this field is 50 characters.";
    } else if (values.lastName.length < 2) {
      errors.lastName = "The minimum length of this field is two characters.";
    }

    if (!values.email) {
      errors.email = "This field is required.";
    } else if (!isValidEmail(values.email)) {
      errors.email = "This must be a valid email address.";
    } else if (values.email.length > 50) {
      errors.email = "The maximum length of this field is 50 characters.";
    }

    if (!values.username) {
      errors.username = "This field is required.";
    } else if (values.username.length > 50) {
      errors.username = "The maximum length of this field is 50 characters.";
    } else if (values.username.length < 5) {
      errors.username = "The minimum length of this field is five characters.";
    }

    if (!values.confirmPassword) {
      errors.confirmPassword = "This field is required.";
    }

    return errors;
  };

  return (
    <div className={styles.AccountInformation}>
      <Text
        type={textTypes.HEADING.SM}
        className={styles.AccountInformation_title}
      >
        Account Information
      </Text>

      <Formik
        initialValues={{
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email,
          username: user.username,
          confirmPassword: "",
        }}
        onSubmit={async (values, { setErrors }) => {
          const currentFormValues = {
            firstName: values.firstName,
            lastName: values.lastName,
            email: values.email,
            username: values.username,
            password: values.confirmPassword,
          };

          const errors = validate(values);
          if (!isEmpty(errors)) {
            setErrors(errors);
            return;
          }

          const { responseCode: updateUserResponseCode, errors: updateErrors } =
            await updateUser(user.id, currentFormValues);

          const updateUserCallbacks = {
            updated: () => {
              alert.success("Account updated successfully.");

              loginUpdate({
                ...user,
                ...currentFormValues,
              });
            },
            invalidFields: () => {
              alert.error("Invalid fields.");
              errors.email = updateErrors.email;
              errors.username = updateErrors.username;
              errors.confirmPassword = updateErrors.password;
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
              className={styles.AccountInformation_input}
              placeholder="First Name*"
              name="firstName"
              icon="contact_mail"
              value={values.firstName}
              error={errors.firstName}
              onChange={(e) => setFieldValue("firstName", e.target.value)}
            />

            <ControlledInput
              className={styles.AccountInformation_input}
              placeholder="Last Name*"
              name="lastName"
              icon="contact_mail"
              value={values.lastName}
              error={errors.lastName}
              onChange={(e) => setFieldValue("lastName", e.target.value)}
            />

            <ControlledInput
              className={styles.AccountInformation_input}
              placeholder="Email Address*"
              name="email"
              icon="email"
              value={values.email}
              error={errors.email}
              onChange={(e) => setFieldValue("email", e.target.value)}
            />

            <ControlledInput
              className={styles.AccountInformation_input}
              placeholder="Username*"
              name="username"
              icon="account_circle"
              value={values.username}
              error={errors.username}
              onChange={(e) => setFieldValue("username", e.target.value)}
            />

            <ControlledInput
              className={styles.AccountInformation_input}
              kind={inputKinds.PASSWORD}
              placeholder="Confirm Password*"
              name="confirmPassword"
              icon="vpn_key"
              value={values.confirmPassword}
              error={errors.confirmPassword}
              onChange={(e) => setFieldValue("confirmPassword", e.target.value)}
            />

            <div className={styles.AccountInformation_buttonGroup}>
              <Button
                className={styles.AccountInformation_buttonGroup_updateButton}
                kind={buttonKinds.SUBMIT}
                disabled={isUserUpdating}
                onClick={() => {}}
              >
                <span
                  className={styles.AccountInformation_buttonGroup_buttonText}
                >
                  Update
                  {isVerifyingPassword && isUserUpdating && (
                    <Spinner
                      size={spinnerSizes.XS}
                      colorName={colorNames.WHITE}
                      className={styles.AccountInformation_buttonGroup_spinner}
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

export default AccountInformation;
