import { useContext } from "react";
import { Route, Redirect } from "react-router-dom";

import { UserContext } from "src/contexts";

const PrivateRoute = ({ ...rest }) => {
  const { user } = useContext(UserContext);

  let userId = null;

  if (user?.id) {
    userId = user.id;
  }

  // the page can be accessed by the user
  if (userId != null) {
    return <Route {...rest} />;
  }

  if (userId != null) {
    <Route name="Contacts" render={() => <Redirect to="/user/contacts" />} />;
  }

  return <Route name="Login" render={() => <Redirect to="/login" />} />;
};

export default PrivateRoute;
