import cn from "classnames";
import PropTypes from "prop-types";

import Icon from "../Icon";
import { buttonKinds, buttonTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const Button = ({
  children,
  type,
  kind,
  className,
  onClick,
  disabled,
  tabIndex,
  icon,
  iconPosition,
}) => (
  <button
    data-test="button"
    type={kind}
    className={cn(className, styles[`Button___${type}`], {
      [styles.Button___withIcon]: icon !== null,
    })}
    onClick={onClick}
    disabled={disabled}
    tabIndex={tabIndex}
  >
    {iconPosition === "left" && icon && (
      <Icon icon={icon} className={styles.Button___withIcon_iconLeft} />
    )}

    {children}

    {iconPosition === "right" && icon && (
      <Icon icon={icon} className={styles.Button___withIcon_iconRight} />
    )}
  </button>
);

Button.defaultProps = {
  type: buttonTypes.PRIMARY.GREEN,
  kind: buttonKinds.BUTTON,
  className: null,
  disabled: false,
  tabIndex: 0,
  icon: null,
  iconPosition: "left",
};

Button.propTypes = {
  children: PropTypes.any.isRequired,
  type: PropTypes.oneOf([
    buttonTypes.PRIMARY.VIOLET,
    buttonTypes.PRIMARY.BLUE,
    buttonTypes.PRIMARY.RED,
    buttonTypes.PRIMARY.GREEN,
    buttonTypes.PRIMARY.YELLOW,
    buttonTypes.SECONDARY.VIOLET,
    buttonTypes.SECONDARY.BLUE,
    buttonTypes.SECONDARY.RED,
    buttonTypes.SECONDARY.GREEN,
    buttonTypes.SECONDARY.YELLOW,
    buttonTypes.TEXT.VIOLET,
    buttonTypes.TEXT.BLUE,
    buttonTypes.TEXT.RED,
    buttonTypes.TEXT.GREEN,
    buttonTypes.TEXT.YELLOW,
    buttonTypes.TERTIARY,
  ]),
  kind: PropTypes.oneOf([
    buttonKinds.BUTTON,
    buttonKinds.SUBMIT,
    buttonKinds.RESET,
  ]),
  className: PropTypes.string,
  onClick: PropTypes.func.isRequired,
  disabled: PropTypes.bool,
  tabIndex: PropTypes.number,
  icon: PropTypes.string,
  iconPosition: PropTypes.oneOf(["left", "right"]),
};

export default Button;
