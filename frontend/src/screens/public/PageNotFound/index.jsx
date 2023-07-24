import NoResults from "src/assets/images/Misc/no-results.webp";

import { ButtonLink, Text } from "src/components";
import { buttonTypes, colorClasses, textTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const PageNotFound = () => {
  return (
    <div className={styles.PageNotFound}>
      <img
        src={NoResults}
        alt="No Contacts"
        className={styles.PageNotFound_image}
      />
      <Text
        colorClass={colorClasses.NEUTRAL["400"]}
        type={textTypes.HEADING.XXL}
        className={styles.PageNotFound_404}
      >
        404
      </Text>
      <Text colorClass={colorClasses.RED["400"]} type={textTypes.HEADING.XS}>
        PAGE NOT FOUND
      </Text>

      <ButtonLink
        to={`/login`}
        type={buttonTypes.PRIMARY.GREEN}
        icon="arrow_back"
        className={styles.PageNotFound_button}
      >
        Back to Main Page
      </ButtonLink>
    </div>
  );
};

export default PageNotFound;
