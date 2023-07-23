import cn from "classnames";
import PropTypes from "prop-types";

import { shineTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const Shine = ({ className, type }) => (
  <div className={cn(styles.Shine, className, styles[`Shine___${type}`])} />
);

Shine.defaultProps = {
  className: null,
  type: shineTypes.STRAIGHT,
};

Shine.propTypes = {
  className: PropTypes.string,
  type: PropTypes.oneOf(Object.values(shineTypes)),
};

export default Shine;
