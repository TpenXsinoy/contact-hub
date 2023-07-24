import cn from "classnames";
import PropTypes from "prop-types";

import styles from "./styles.module.scss";

const Card = ({ className, children }) => (
  <div className={cn(styles.Card, className)}>{children}</div>
);

Card.defaultProps = {
  className: null,
  children: null,
};

Card.propTypes = {
  className: PropTypes.string,
  children: PropTypes.any,
};

export default Card;
