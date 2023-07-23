import cn from "classnames";
import PropTypes from "prop-types";
import { Link } from "react-router-dom";

import Icon from "../../Icon";
import { buttonTypes } from "src/app-globals";

import styles from "../styles.module.scss";

const ButtonLink = ({
  children,
  type,
  className,
  to,
  disabled,
  tabIndex,
  icon,
  iconPosition,
  onClick,
}) => (
  <Link
    className={cn(className, styles[`Button___${type}`], {
      [styles.Button___withIcon]: icon !== null,
      [styles.Button___disabled]: disabled,
    })}
    tabIndex={tabIndex}
    to={to}
    onClick={onClick}
  >
    {iconPosition === "left" && icon && (
      <Icon icon={icon} className={styles.Button___withIcon_iconLeft} />
    )}

    {children}

    {iconPosition === "right" && icon && (
      <Icon icon={icon} className={styles.Button___withIcon_iconRight} />
    )}
  </Link>
);

ButtonLink.defaultProps = {
  type: buttonTypes.PRIMARY.VIOLET,
  className: null,
  tabIndex: 0,
  disabled: false,
  icon: null,
  iconPosition: "left",
  onClick: () => {},
};

ButtonLink.propTypes = {
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
  className: PropTypes.string,
  to: PropTypes.string.isRequired,
  disabled: PropTypes.bool,
  tabIndex: PropTypes.number,
  icon: PropTypes.string,
  iconPosition: PropTypes.oneOf(["left", "right"]),
  onClick: PropTypes.func,
};

export default ButtonLink;
