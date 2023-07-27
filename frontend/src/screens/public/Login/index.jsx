import { useState, useContext } from "react";
import Cookies from "universal-cookie";
import { Formik } from "formik";
import isEmpty from "lodash/isEmpty";

import {
  Card,
  ControlledInput,
  Button,
  ButtonLink,
  Spinner,
  Text,
} from "src/components";

import {
  buttonKinds,
  buttonTypes,
  colorClasses,
  colorNames,
  inputKinds,
  spinnerSizes,
  textTypes,
} from "src/app-globals";
import { UserContext } from "src/contexts";
import { UsersService, TokensService } from "src/services";
import Logo from "src/assets/images/Logo/logo.png";

import ForgotPasswordModals from "./ForgotPasswordModals";

import styles from "./styles.module.scss";

const validate = (values) => {
  const errors = {};

  if (!values.username) {
    errors.username = "This field is required.";
  }

  if (!values.password) {
    errors.password = "This field is required.";
  }

  return errors;
};

const Login = () => {
  const { loginUpdate } = useContext(UserContext);
  const cookies = new Cookies();
  const [isLoggingIn, setIsLoggingIn] = useState(false);
  const [isForgotPasswordToggled, toggleIsForgotPassword] = useState(false);

  return (
    <>
      <Card className={styles.Login}>
        <div className={styles.Login_header}>
          <img
            src={Logo}
            className={styles.Login_header_logo}
            alt="Contact Hub Logo"
          />

          <div className={styles.Login_header_headingTextWrapper}>
            <Text
              type={textTypes.SPAN.MD}
              className={styles.Login_header_headingTextWrapper_headingText}
            >
              Sign In with Contact Hub
            </Text>
          </div>
        </div>

        <div className={styles.Login_content}>
          <Formik
            initialValues={{
              username: "",
              password: "",
            }}
            onSubmit={async (values, { setErrors }) => {
              const currentFormValues = {
                username: values.username,
                password: values.password,
              };

              const errors = validate(values);
              if (!isEmpty(errors)) {
                setErrors(errors);
                return;
              }

              setIsLoggingIn(true);

              // Login the user
              try {
                const { data: loginResponse } = await UsersService.login(
                  currentFormValues
                );

                // Call the Acquire Tokens endpoint to set the tokens in to the cookies
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
                loginUpdate(loginResponse);

                setIsLoggingIn(false);
              } catch (error) {
                const status = error.response.status;

                switch (status) {
                  case 400:
                    setIsLoggingIn(false);
                    setErrors({
                      overall: "Invalid username and/or password.",
                    });
                    break;
                  case 404:
                    setIsLoggingIn(false);
                    setErrors({
                      overall: "Invalid username and/or password.",
                    });
                    break;
                  case 500:
                    setIsLoggingIn(false);
                    setErrors({
                      overall: "Oops, something went wrong.",
                    });
                    break;
                  default:
                    break;
                }
                setIsLoggingIn(false);
              }
            }}
          >
            {({ errors, values, handleSubmit, setFieldValue }) => (
              <form onSubmit={handleSubmit}>
                <ControlledInput
                  className={styles.Login_content_input}
                  placeholder="Username"
                  name="username"
                  icon="account_circle"
                  value={values.username}
                  error={errors.username}
                  onChange={(e) => setFieldValue("username", e.target.value)}
                />
                <ControlledInput
                  kind={inputKinds.PASSWORD}
                  className={styles.Login_content_input}
                  placeholder="Password"
                  name="password"
                  icon="vpn_key"
                  value={values.password}
                  error={errors.password}
                  onChange={(e) => setFieldValue("password", e.target.value)}
                />
                {errors.overall && (
                  <Text
                    className={styles.Login_content_input_errorMessage}
                    type={textTypes.BODY.XS}
                    colorClass={colorClasses.RED["400"]}
                  >
                    {errors.overall}
                  </Text>
                )}

                <Button
                  type={buttonTypes.TEXT.VIOLET}
                  className={styles.Login_content_forgotPassword}
                  onClick={() => toggleIsForgotPassword(true)}
                >
                  Forgot Password?
                </Button>

                <div className={styles.Login_content_buttonGroup}>
                  <Button
                    kind={buttonKinds.SUBMIT}
                    icon="lock_open"
                    disabled={isLoggingIn}
                    onClick={() => {}}
                  >
                    <span
                      className={styles.Login_content_buttonGroup_buttonText}
                    >
                      Sign In
                      {isLoggingIn && (
                        <Spinner
                          size={spinnerSizes.XS}
                          colorName={colorNames.WHITE}
                          className={styles.Login_content_buttonGroup_spinner}
                        />
                      )}
                    </span>
                  </Button>

                  <ButtonLink
                    type={buttonTypes.SECONDARY.GREEN}
                    to="/signup"
                    icon="person"
                    disabled={isLoggingIn}
                  >
                    Sign Up
                  </ButtonLink>
                </div>
              </form>
            )}
          </Formik>
        </div>
      </Card>

      {isForgotPasswordToggled && (
        <ForgotPasswordModals
          isOpen={isForgotPasswordToggled}
          handleClose={() => toggleIsForgotPassword(false)}
        />
      )}
    </>
  );
};

export default Login;
