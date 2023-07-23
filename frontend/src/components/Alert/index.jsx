import cn from "classnames";
import PropTypes from "prop-types";

import Icon from "../Icon";
import Text from "../Text";

import { colorClasses, textTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const Alert = ({ style, options, message }) => (
  <div
    className={cn(styles.Alert, styles[`Alert___${options.type}`])}
    style={style}
  >
    {options.type === "info" && (
      <Icon icon="info" className={styles.Alert_icon} />
    )}
    {options.type === "success" && (
      <Icon icon="check" className={styles.Alert_icon} />
    )}
    {options.type === "error" && (
      <Icon icon="error" className={styles.Alert_icon} />
    )}

    <Text
      type={textTypes.HEADING.XXXS}
      colorClass={colorClasses.NEUTRAL["0"]}
      className={styles.Alert_message}
    >
      {message}
    </Text>
  </div>
);

Alert.defaultProps = {
  style: {},
  options: {},
};

Alert.propTypes = {
  style: PropTypes.object,
  options: PropTypes.object,
  message: PropTypes.string.isRequired,
};

export default Alert;
