import { useState, useContext } from "react";
import { Link } from "react-router-dom";
import Cookies from "universal-cookie";
import { Formik } from "formik";
import isEmpty from "lodash/isEmpty";
import {
  Card,
  ControlledInput,
  Button,
  Grid,
  Spinner,
  Text,
} from "src/components";

import {
  buttonKinds,
  colorNames,
  inputKinds,
  spinnerSizes,
  textTypes,
} from "src/app-globals";

import { isValidEmail, isValidPassword } from "src/utils/string";
import { UserContext } from "src/contexts";
import { UsersService, TokensService } from "src/services";
import Logo from "src/assets/images/Logo/logo.png";

import styles from "./styles.module.scss";

const SignUp = () => {
  const [isSigningUp, setIsSigningUp] = useState(false);
  const { loginUpdate } = useContext(UserContext);
  const cookies = new Cookies();

  const validate = (values) => {
    const errors = {};

    if (!values.firstName) {
      errors.firstName = "This field is required.";
    } else if (values.firstName.length > 50) {
      errors.firstName = "The maximum length of this field is 50 characters.";
    }

    if (!values.lastName) {
      errors.lastName = "This field is required.";
    } else if (values.lastName.length > 50) {
      errors.lastName = "The maximum length of this field is 50 characters.";
    }

    if (!values.email) {
      errors.email = "This field is required.";
    } else if (!isValidEmail(values.email)) {
      errors.email = "This must be a valid email address.";
    }

    if (!values.username) {
      errors.username = "This field is required.";
    } else if (values.username.length > 50) {
      errors.username = "The maximum length of this field is 50 characters.";
    }

    if (!values.password) {
      errors.password = "This field is required.";
    } else if (values.password.length > 50) {
      errors.password = "The maximum length of this field is 50 characters.";
    } else if (!isValidPassword(values.password)) {
      errors.password =
        "Password must have atleast 8 characters, 1 uppercase, 1 lowercase, 1 number and 1 special character.";
    }

    if (!values.confirmPassword) {
      errors.confirmPassword = "This field is required.";
    } else if (values.password && values.password !== values.confirmPassword) {
      errors.confirmPassword = "This must match with your password.";
    }

    return errors;
  };

  return (
    <Card className={styles.SignUp}>
      <div className={styles.SignUp_header}>
        <img
          src={Logo}
          className={styles.SignUp_header_logo}
          alt="Codyssey Logo"
        />

        <div className={styles.SignUp_header_headingTextWrapper}>
          <Text
            type={textTypes.SPAN.MD}
            className={styles.SignUp_header_headingTextWrapper_headingText}
          >
            Sign Up with Contact Hub
          </Text>
        </div>
      </div>

      <div className={styles.SignUp_content}>
        <Formik
          initialValues={{
            firstName: "",
            lastName: "",
            email: "",
            username: "",
            password: "",
            confirmPassword: "",
          }}
          onSubmit={async (values, { setErrors }) => {
            const currentFormValues = {
              firstName: values.firstName,
              lastName: values.lastName,
              email: values.email,
              username: values.username,
              password: values.password,
            };

            const errors = validate(values);
            if (!isEmpty(errors)) {
              setErrors(errors);
              return;
            }

            setIsSigningUp(true);

            // Sign up the user
            try {
              const { data: SignUpResponse } = await UsersService.signup(
                currentFormValues
              );

              // If the creation of the user is successful
              // then we need to log the user in

              // Call the Acquire Tokens endpoint to set the tokens
              // in to the cookies
              const { data: acquireResponse } = await TokensService.acquire({
                username: currentFormValues.username,
                password: currentFormValues.password,
              });

              cookies.set("accessToken", acquireResponse.accessToken, {
                path: "/",
              });
              cookies.set("refreshToken", acquireResponse.refreshToken, {
                path: "/",
              });

              // Update login
              loginUpdate(SignUpResponse);

              setIsSigningUp(false);
            } catch (error) {
              const responseData = error.response.data;

              if (responseData.includes("Username")) {
                setErrors({
                  username: "Username is already taken.",
                });

                setIsSigningUp(false);
              } else if (responseData.includes("Email")) {
                setErrors({
                  email: "Email is already taken.",
                });

                setIsSigningUp(false);
              }

              const status = error.response.status;
              const dataErrors = responseData.errors;

              switch (status) {
                case 400:
                  if (dataErrors.Email) {
                    setErrors({
                      email: dataErrors.email,
                    });
                  }

                  if (dataErrors.Password) {
                    setErrors({
                      password: dataErrors.Password,
                    });
                  }
                  break;

                case 500:
                  setErrors({
                    overall: "Oops, something went wrong.",
                  });
                  break;
                default:
                  break;
              }

              setIsSigningUp(false);
            }
          }}
        >
          {({ errors, values, handleSubmit, setFieldValue }) => (
            <form onSubmit={handleSubmit}>
              <ControlledInput
                placeholder="First Name*"
                name="firstName"
                icon="contact_mail"
                value={values.firstName}
                error={errors.firstName}
                onChange={(e) => setFieldValue("firstName", e.target.value)}
              />

              <ControlledInput
                className={styles.SignUp_content_withMargin}
                placeholder="Last Name*"
                name="lastName"
                icon="contact_mail"
                value={values.lastName}
                error={errors.lastName}
                onChange={(e) => setFieldValue("lastName", e.target.value)}
              />

              <ControlledInput
                className={styles.SignUp_content_withMargin}
                placeholder="Email Address*"
                name="email"
                icon="email"
                value={values.email}
                error={errors.email}
                onChange={(e) => setFieldValue("email", e.target.value)}
              />

              <ControlledInput
                className={styles.SignUp_content_withMargin}
                placeholder="Username*"
                name="username"
                icon="account_circle"
                value={values.username}
                error={errors.username}
                onChange={(e) => setFieldValue("username", e.target.value)}
              />

              <Grid className={styles.SignUp_content_withMargin}>
                <ControlledInput
                  kind={inputKinds.PASSWORD}
                  placeholder="Password*"
                  name="password"
                  icon="vpn_key"
                  value={values.password}
                  error={errors.password}
                  onChange={(e) => setFieldValue("password", e.target.value)}
                />
                <ControlledInput
                  kind={inputKinds.PASSWORD}
                  placeholder="Confirm Password*"
                  name="confirmPassword"
                  icon="vpn_key"
                  value={values.confirmPassword}
                  error={errors.confirmPassword}
                  onChange={(e) =>
                    setFieldValue("confirmPassword", e.target.value)
                  }
                />
              </Grid>

              <div className={styles.SignUp_content_buttonGroup}>
                <Button
                  kind={buttonKinds.SUBMIT}
                  icon="person"
                  disabled={isSigningUp}
                  onClick={() => {}}
                >
                  <span
                    className={styles.SignUp_content_buttonGroup_buttonText}
                  >
                    Sign Up
                    {isSigningUp && (
                      <Spinner
                        size={spinnerSizes.XS}
                        colorName={colorNames.WHITE}
                        className={styles.SignUp_content_buttonGroup_spinner}
                      />
                    )}
                  </span>
                </Button>
              </div>
            </form>
          )}
        </Formik>
      </div>

      <div className={styles.SignUp_footer}>
        <Text>
          Already have an account?{" "}
          <Link
            to="/login"
            className={styles.SignUp_footer_signIn}
            onClick={isSigningUp ? (e) => e.preventDefault() : () => {}}
          >
            Sign In
          </Link>
        </Text>
      </div>
    </Card>
  );
};

export default SignUp;
