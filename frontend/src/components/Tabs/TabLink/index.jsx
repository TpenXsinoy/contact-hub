import { Link } from "react-router-dom";
import cn from "classnames";
import PropTypes from "prop-types";

import IconButton from "../../Button/IconButton";
import { iconButtonTypes, tabTypes } from "src/app-globals";

import styles from "../tabs.module.scss";

const TabLink = ({
  className,
  children,
  type,
  active,
  to,
  closeAction,
  id,
}) => (
  <Link
    data-test="tabLink"
    className={cn(className, {
      [styles[`Tab___${type}___active`]]: active,
      [styles[`Tab___${type}`]]: !active,
      [styles.Tab___withClose]: closeAction && active,
    })}
    to={to}
    id={id}
  >
    {children}
    {closeAction && active && (
      <IconButton
        data-test="closeButton"
        className={styles.Tab_close}
        type={iconButtonTypes.SOLID.XS}
        icon="close"
        onClick={closeAction}
      />
    )}
  </Link>
);

TabLink.defaultProps = {
  className: null,
  active: false,
  type: tabTypes.HORIZONTAL.LG,
  to: "#",
  closeAction: null,
  id: null,
};

TabLink.propTypes = {
  className: PropTypes.string,
  active: PropTypes.bool,
  to: PropTypes.string,
  children: PropTypes.node.isRequired,
  type: PropTypes.oneOf([
    tabTypes.HORIZONTAL.LG,
    tabTypes.HORIZONTAL.SM,
    tabTypes.VERTICAL.LG,
    tabTypes.VERTICAL.SM,
  ]),
  closeAction: PropTypes.func,
  id: PropTypes.string,
};

export default TabLink;
