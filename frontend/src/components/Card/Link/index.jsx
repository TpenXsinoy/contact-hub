import cn from "classnames";
import PropTypes from "prop-types";
import { Link } from "react-router-dom";

import styles from "../styles.module.scss";

const CardLink = ({ id, children, className, to, target }) => (
  <Link
    className={cn(styles.Card, styles.Card___clickable, className)}
    id={id}
    tabIndex={0}
    target={target}
    to={to}
  >
    {children}
  </Link>
);

CardLink.defaultProps = {
  id: null,
  children: null,
  className: null,
  to: "#",
  target: null,
};

CardLink.propTypes = {
  id: PropTypes.string,
  children: PropTypes.any,
  className: PropTypes.string,
  to: PropTypes.string,
  target: PropTypes.string,
};

CardLink.displayName = "CardLink";

export default CardLink;
