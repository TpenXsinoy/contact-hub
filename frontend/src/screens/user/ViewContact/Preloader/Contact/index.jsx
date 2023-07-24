import { Card, Shine, Separator } from "src/components";

import styles from "./styles.module.scss";

const PreloaderContact = () => {
  return (
    <Card className={styles.PreloaderContact}>
      <div className={styles.PreloaderContact_header}>
        <Shine className={styles.PreloaderContact_header_shine} />
        <Shine className={styles.PreloaderContact_header_shine} />
        <Shine className={styles.PreloaderContact_header_shine} />
        <Shine className={styles.PreloaderContact_header_shine} />
        <Shine className={styles.PreloaderContact_header_shine} />
        <Shine className={styles.PreloaderContact_header_shine} />
      </div>

      <Separator className={styles.PreloaderContact_separator} />

      <div className={styles.PreloaderContact_body}>
        <Shine className={styles.PreloaderContact_body_shine} />
        <Shine className={styles.PreloaderContact_body_shine} />
        <Shine className={styles.PreloaderContact_body_shine} />
        <Shine className={styles.PreloaderContact_body_shine} />
        <Shine className={styles.PreloaderContact_body_shine} />
        <Shine className={styles.PreloaderContact_body_shine} />
      </div>
    </Card>
  );
};

export default PreloaderContact;
