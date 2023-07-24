import PropTypes from "prop-types";
import { Link } from "react-router-dom";
import cn from "classnames";

import Icon from "../../Icon";
import { iconButtonTypes } from "src/app-globals";

import styles from "../icon.module.scss";

const IconLink = ({
  icon,
  className,
  iconClassName,
  style,
  to,
  disabled,
  type,
  tabIndex,
}) => (
  <Link
    className={cn(className, styles[`IconButton___${type}`], {
      [styles.Button___withIcon]: icon !== null,
      [styles.Button___disabled]: disabled,
    })}
    tabIndex={tabIndex}
    to={to}
  >
    <Icon
      icon={icon}
      className={cn(styles.IconButton_icon, iconClassName)}
      style={style}
    />
  </Link>
);

IconLink.defaultProps = {
  className: null,
  style: null,
  iconClassName: null,
  disabled: false,
  type: iconButtonTypes.SOLID.SM,
  tabIndex: 0,
};

IconLink.propTypes = {
  type: PropTypes.oneOf([
    iconButtonTypes.SOLID.LG,
    iconButtonTypes.SOLID.MD,
    iconButtonTypes.SOLID.SM,
    iconButtonTypes.SOLID.XS,
    iconButtonTypes.OUTLINE.LG,
    iconButtonTypes.OUTLINE.MD,
    iconButtonTypes.OUTLINE.SM,
    iconButtonTypes.OUTLINE.XS,
  ]),
  className: PropTypes.string,
  icon: PropTypes.string.isRequired,
  style: PropTypes.object,
  to: PropTypes.string.isRequired,
  iconClassName: PropTypes.string,
  disabled: PropTypes.bool,
  tabIndex: PropTypes.number,
};

export default IconLink;
