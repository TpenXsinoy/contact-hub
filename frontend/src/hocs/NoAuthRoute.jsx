import { useContext } from "react";
import { Route, Redirect } from "react-router-dom";
import { UserContext } from "src/contexts";

const NoAuthRoute = ({ ...rest }) => {
  const { user } = useContext(UserContext);
  let userId = null;

  if (user?.id) {
    userId = user.id;
  }

  // Redirect to specific pages if userId exists
  if (userId) {
    return (
      <Route name="Contacts" render={() => <Redirect to="/user/contacts" />} />
    );
  }

  // the page can be accessed by the user
  return <Route {...rest} />;
};

export default NoAuthRoute;
