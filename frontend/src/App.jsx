import React, { useState } from "react";
import { BrowserRouter, Switch, Route, Redirect } from "react-router-dom";
import { transitions, positions, Provider as AlertProvider } from "react-alert";
import Cookies from "universal-cookie";
import { Alert } from "src/components";
import { UserContext } from "src/contexts";
import { PrivateRoute } from "src/hocs";
import { LoginSignup, User } from "src/screen-wrappers";
import { Logout } from "src/screens/public";

import "src/styles/App.scss";

const cookies = new Cookies();

// Alert configuration
const alertOptions = {
  position: positions.BOTTOM_RIGHT,
  timeout: 4000,
  offset: "0 20px 10px",
  transition: transitions.SCALE,
};

function App() {
  const [user, setUser] = useState(cookies.get("user"));

  const loginUpdate = (userData) => {
    cookies.set("user", userData, {
      path: "/",
    });

    setUser(userData);
  };

  const loginRestart = () => {
    cookies.remove("user", {
      path: "/",
    });
    cookies.remove("accessToken", {
      path: "/",
    });
    cookies.remove("refreshToken", {
      path: "/",
    });

    setUser(null);
  };

  return (
    <BrowserRouter basename="/">
      <React.Suspense fallback={<div>Loading...</div>}>
        <UserContext.Provider value={{ user, loginUpdate, loginRestart }}>
          <AlertProvider template={Alert} {...alertOptions}>
            <Switch>
              <PrivateRoute
                path="/logout"
                name="Logout"
                render={(props) => <Logout {...props} />}
              />
              <PrivateRoute
                path="/user"
                name="User"
                render={(props) => <User {...props} />}
              />
              <Route
                path="/"
                name="Login/Sign Up"
                render={(props) => <LoginSignup {...props} />}
              />

              <Redirect from="/" to="/login" />
              <Redirect to="/page-not-found" />
            </Switch>
          </AlertProvider>
        </UserContext.Provider>
      </React.Suspense>
    </BrowserRouter>
  );
}

export default App;
