import cn from "classnames";
import PropTypes from "prop-types";

import { colorNames, spinnerSizes } from "src/app-globals";

import styles from "./styles.module.scss";

const Spinner = ({ className, colorName, size }) => (
  <div className={cn(styles.Spinner_container, className)}>
    <svg className={styles[`Spinner___${size}`]} viewBox="25 25 50 50">
      <circle
        className={styles[`Spinner_circle___${colorName}`]}
        cx="50"
        cy="50"
        r="20"
      />
    </svg>
  </div>
);

Spinner.defaultProps = {
  className: null,
  colorName: colorNames.VIOLET,
  size: spinnerSizes.LG,
};

Spinner.propTypes = {
  className: PropTypes.string,
  colorName: PropTypes.oneOf([
    colorNames.GRAY,
    colorNames.VIOLET,
    colorNames.WHITE,
  ]),
  size: PropTypes.oneOf([
    spinnerSizes.LG,
    spinnerSizes.MD,
    spinnerSizes.SM,
    spinnerSizes.XS,
  ]),
};

export default Spinner;
