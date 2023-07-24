import PropTypes from "prop-types";
import { NavLink } from "react-router-dom";

import { Icon } from "src/components";

import styles from "./styles.module.scss";

const SidebarLink = ({ to, icon, label }) => (
  <NavLink
    to={to}
    className={styles.SidebarLink}
    activeClassName={styles.SidebarLink___active}
  >
    <Icon icon={icon} className={styles.SidebarLink_icon} />
    {label}
  </NavLink>
);

SidebarLink.propTypes = {
  to: PropTypes.string.isRequired,
  icon: PropTypes.string.isRequired,
  label: PropTypes.string.isRequired,
};

export default SidebarLink;
