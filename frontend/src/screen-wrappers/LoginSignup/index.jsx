import React from "react";
import { Switch, Redirect } from "react-router-dom";
import NoAuthRoute from "src/hocs/NoAuthRoute";
import { Login, SignUp } from "src/screens/public";

import styles from "./styles.module.scss";

const LoginSignupContainer = () => {
  return (
    <div className={styles.LoginSignupContainer}>
      <React.Suspense fallback={<div>Loading...</div>}>
        <Switch>
          <NoAuthRoute
            path="/login"
            name="Login"
            exact
            render={(props) => <Login {...props} />}
          />

          <NoAuthRoute
            path="/signup"
            name="Signup"
            exact
            render={(props) => <SignUp {...props} />}
          />

          <Redirect from="*" to="/login" />
        </Switch>
      </React.Suspense>
    </div>
  );
};
export default LoginSignupContainer;
