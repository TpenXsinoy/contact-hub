import cn from "classnames";
import PropTypes from "prop-types";

import styles from "./styles.module.scss";

const Separator = ({ className }) => (
  <hr className={cn(styles.Separator, className)} />
);

Separator.propTypes = {
  className: PropTypes.string,
};

Separator.defaultProps = {
  className: null,
};

export default Separator;
