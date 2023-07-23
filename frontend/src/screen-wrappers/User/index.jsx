import React, { useState, useEffect } from "react";
import { useLocation, Switch, Redirect, Route } from "react-router-dom";

import Navbar from "../Navbar";
import Sidebar from "../Sidebar";
import {
  Contacts,
  CreateAddress,
  CreateContact,
  Settings,
  UpdateAddress,
  UpdateContact,
  ViewContact,
} from "src/screens/user";

import styles from "./styles.module.scss";
import USER_ROUTES from "../constants/userRoutes";

const UserContainer = () => {
  const location = useLocation();

  const [isSidebarToggled, toggleSidebar] = useState(true);

  const handleToggleSidebar = (isToggled) => {
    toggleSidebar(isToggled);
  };

  useEffect(() => {
    handleToggleSidebar(true);
  }, [location]);

  return (
    <>
      <Navbar
        isSidebarToggled={isSidebarToggled}
        handleToggleSidebar={handleToggleSidebar}
      />

      <Sidebar isToggled={isSidebarToggled} />

      <div className={styles.UserContainer}>
        <React.Suspense fallback={<div>Loading...</div>}>
          <Switch>
            <Route
              path={USER_ROUTES.CONTACTS}
              name="Contacts"
              exact
              render={(props) => <Contacts {...props} />}
            />

            <Route
              path={USER_ROUTES.CREATE_CONTACT}
              name="Create Contact"
              exact
              render={(props) => <CreateContact {...props} />}
            />

            <Route
              path={USER_ROUTES.UPDATE_CONTACT}
              name="Update Contact"
              exact
              render={(props) => <UpdateContact {...props} />}
            />

            <Route
              path={USER_ROUTES.VIEW_CONTACT}
              name="View Contact"
              exact
              render={(props) => <ViewContact {...props} />}
            />

            <Route
              path={USER_ROUTES.CREATE_ADDRESS}
              name="Create Address"
              exact
              render={(props) => <CreateAddress {...props} />}
            />

            <Route
              path={USER_ROUTES.UPDATE_ADDRESS}
              name="Update Address"
              exact
              render={(props) => <UpdateAddress {...props} />}
            />

            <Route
              path={USER_ROUTES.SETTINGS}
              name="Settings"
              exact
              render={(props) => <Settings {...props} />}
            />

            <Redirect from="*" to={USER_ROUTES.CONTACTS} />
          </Switch>
        </React.Suspense>
      </div>
    </>
  );
};

export default UserContainer;
