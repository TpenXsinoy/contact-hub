import { useContext, useEffect } from "react";
import { UserContext } from "src/contexts";

const Logout = () => {
  const { user, loginRestart } = useContext(UserContext);

  useEffect(() => {
    if (!user?.hasOpenedTabletView) {
      loginRestart();
    }
  }, []);

  return null;
};

export default Logout;
