import cn from "classnames";
import PropTypes from "prop-types";

import IconButton from "../../Button/IconButton";
import { iconButtonTypes, tabTypes } from "src/app-globals";

import styles from "../tabs.module.scss";

const TabButton = ({
  className,
  children,
  type,
  active,
  onClick,
  closeAction,
  id,
}) => (
  <button
    id={id}
    data-test="tabButton"
    className={cn(className, {
      [styles[`Tab___${type}___active`]]: active,
      [styles[`Tab___${type}`]]: !active,
      [styles.Tab___withClose]: closeAction && active,
    })}
    onClick={onClick}
    type="button"
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
  </button>
);

TabButton.defaultProps = {
  className: null,
  active: false,
  type: tabTypes.HORIZONTAL.LG,
  closeAction: null,
  id: null,
};

TabButton.propTypes = {
  className: PropTypes.string,
  active: PropTypes.bool,
  onClick: PropTypes.func.isRequired,
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

export default TabButton;
