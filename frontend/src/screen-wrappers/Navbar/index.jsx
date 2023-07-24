import { useState, useRef, useContext } from "react";
import cn from "classnames";
import { Link } from "react-router-dom";
import PropTypes from "prop-types";

import { UserContext } from "src/contexts";
import { Card, Icon, IconButton, Text } from "src/components";
import { colorClasses, iconButtonTypes, textTypes } from "src/app-globals";

import { useOnClickOutside } from "src/hooks";
import IconLogoWhite from "src/assets/images/Logo/icon-white.png";
import { getEmailUserName } from "src/utils/string";
import settingsTabs from "src/screens/user/Settings/constants/settingsTabs";

import styles from "./styles.module.scss";

const Navbar = ({ isSidebarToggled, handleToggleSidebar }) => {
  const ref = useRef();
  const [isDropdownToggled, toggleDropdown] = useState(false);
  useOnClickOutside(ref, () => toggleDropdown(false));

  const { user } = useContext(UserContext);

  return (
    <div className={styles.Navbar}>
      <div className={styles.Navbar_section}>
        <IconButton
          icon="menu"
          type={iconButtonTypes.SOLID.LG}
          onClick={() => {
            handleToggleSidebar(!isSidebarToggled);
          }}
        />
      </div>

      <div className={cn(styles.Navbar_section, styles.Navbar_logo)}>
        <img
          src={IconLogoWhite}
          className={styles.Navbar_logo_icon}
          alt="Contact Hub Logo"
        />

        <Text
          type={textTypes.HEADING.XS}
          colorClass={colorClasses.NEUTRAL["0"]}
          className={styles.Navbar_logo_text}
        >
          Contact Hub
        </Text>

        <div className={styles.Navbar_userType}>
          <Text type="heading___xxxs" className={styles.Navbar_userType_text}>
            {getEmailUserName(user.email)}
          </Text>
        </div>
      </div>

      <div
        ref={ref}
        className={cn(styles.Navbar_section, styles.Navbar_navUser)}
      >
        <>
          <button
            className={styles.Navbar_navUser_button}
            type="button"
            onClick={() => toggleDropdown(!isDropdownToggled)}
            tabIndex={0}
          >
            <span className={styles.Navbar_navUser_name}>{user.lastName}</span>
            <Icon
              icon="keyboard_arrow_down"
              className={styles.Navbar_navUser_caret}
            />
          </button>

          <Card
            className={cn({
              [styles.Navbar_navUser_dropdown]: !isDropdownToggled,
              [styles.Navbar_navUser_dropdown___toggled]: isDropdownToggled,
            })}
          >
            <Link
              className={styles.Navbar_navUser_dropdown_link}
              to={`/user/settings/${settingsTabs.ACCOUNT_INFORMATION.value}`}
              onClick={() => toggleDropdown(!isDropdownToggled)}
            >
              <Icon
                icon="settings"
                className={styles.Navbar_navUser_dropdown_link_icon}
              />
              Settings
            </Link>

            <Link className={styles.Navbar_navUser_dropdown_link} to="/logout">
              <Icon
                icon="exit_to_app"
                className={styles.Navbar_navUser_dropdown_link_icon}
              />
              Logout
            </Link>
          </Card>
        </>
      </div>
    </div>
  );
};

Navbar.propTypes = {
  isSidebarToggled: PropTypes.bool.isRequired,
  handleToggleSidebar: PropTypes.func.isRequired,
};

export default Navbar;
