import cn from "classnames";
import PropTypes from "prop-types";

import { navLinks } from "../constants";
import SidebarLink from "./Link";

import styles from "./styles.module.scss";

const Sidebar = ({ isToggled }) => {
  return (
    <nav
      className={cn(styles.Sidebar, { [styles.Sidebar___toggled]: isToggled })}
    >
      <SidebarLink
        to="/user/contacts"
        label={navLinks.CONTACTS.label}
        icon={navLinks.CONTACTS.icon}
      />
    </nav>
  );
};

Sidebar.propTypes = {
  isToggled: PropTypes.bool.isRequired,
};

export default Sidebar;
