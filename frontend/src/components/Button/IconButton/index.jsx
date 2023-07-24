import PropTypes from "prop-types";
import cn from "classnames";

import Icon from "../../Icon";
import { buttonKinds, iconButtonTypes } from "src/app-globals";

import styles from "../icon.module.scss";

const IconButton = ({
  icon,
  className,
  iconClassName,
  style,
  onClick,
  disabled,
  type,
  tabIndex,
  kind,
}) => (
  <button
    data-test="button"
    type={kind}
    className={cn(className, styles[`IconButton___${type}`])}
    onClick={disabled === false ? onClick || (() => {}) : null}
    disabled={disabled}
    tabIndex={tabIndex}
  >
    <Icon
      icon={icon}
      className={cn(styles.IconButton_icon, iconClassName)}
      style={style}
    />
  </button>
);

IconButton.defaultProps = {
  className: null,
  style: null,
  onClick: null,
  iconClassName: null,
  disabled: false,
  type: iconButtonTypes.SOLID.SM,
  kind: buttonKinds.BUTTON,
  tabIndex: 0,
};

IconButton.propTypes = {
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
  onClick: PropTypes.func,
  iconClassName: PropTypes.string,
  disabled: PropTypes.bool,
  tabIndex: PropTypes.number,
  kind: PropTypes.oneOf([
    buttonKinds.BUTTON,
    buttonKinds.SUBMIT,
    buttonKinds.RESET,
  ]),
};

export default IconButton;
