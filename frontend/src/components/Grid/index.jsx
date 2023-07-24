import cn from "classnames";
import PropTypes from "prop-types";

import { gridTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const Grid = ({ className, type, children }) => (
  <div className={cn(className, styles[`Grid___${type}`])}>{children}</div>
);

Grid.defaultProps = {
  className: null,
  type: gridTypes.TWO,
};

Grid.propTypes = {
  children: PropTypes.any.isRequired,
  className: PropTypes.string,
  type: PropTypes.oneOf([
    gridTypes.ONE,
    gridTypes.TWO,
    gridTypes.THREE,
    gridTypes.FOUR,
    gridTypes.FIVE,
  ]),
};

export default Grid;
