import PropTypes from "prop-types";
import { Link } from "react-router-dom";

import Text from "../Text";
import { textTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const Breadcrumbs = ({ pageTitle, pages }) => (
  <div className={styles.Breadcrumbs}>
    <Text type={textTypes.HEADING.XS}>{pageTitle}</Text>

    <div className={styles.Breadcrumbs_links}>
      {pages.map(({ name, link }, index) => (
        <div className={styles.Breadcrumbs_links_keyWrapper} key={index}>
          <Link to={link} className={styles.Breadcrumbs_links_link}>
            {name}
          </Link>
          <Text className={styles.Breadcrumbs_links_linkSeparator}>/</Text>
        </div>
      ))}
      {pageTitle}
    </div>
  </div>
);

Breadcrumbs.propTypes = {
  // Current page name
  pageTitle: PropTypes.string.isRequired,
  // Pages excluding the current page
  pages: PropTypes.arrayOf(
    PropTypes.shape({
      name: PropTypes.string.isRequired,
      link: PropTypes.string.isRequired,
    })
  ),
};

export default Breadcrumbs;
