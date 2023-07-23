import cn from "classnames";
import PropTypes from "prop-types";

import styles from "./styles.module.scss";

const Icon = ({ icon, className }) => {
  return <i className={cn(styles.Icon, className)}>{icon}</i>;
};

Icon.defaultProps = {
  className: null,
};

Icon.propTypes = {
  className: PropTypes.string,
  icon: PropTypes.string.isRequired,
};

export default Icon;
