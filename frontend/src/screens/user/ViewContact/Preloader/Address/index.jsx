import { Card, Shine, Separator } from "src/components";

import styles from "./styles.module.scss";

const PreloaderAddress = () => (
  <Card className={styles.PreloaderAddress}>
    <div className={styles.PreloaderAddress_header}>
      <Shine className={styles.PreloaderAddress_header_shine} />
      <Shine className={styles.PreloaderAddress_header_shine} />
      <Shine className={styles.PreloaderAddress_header_shine} />
      <Shine className={styles.PreloaderAddress_header_shine} />
      <Shine className={styles.PreloaderAddress_header_shine} />
      <Shine className={styles.PreloaderAddress_header_shine} />
    </div>

    <Separator className={styles.PreloaderAddress_separator} />

    <div className={styles.PreloaderAddress_body}>
      <Shine className={styles.PreloaderAddress_body_shine} />
      <Shine className={styles.PreloaderAddress_body_shine} />
      <Shine className={styles.PreloaderAddress_body_shine} />
      <Shine className={styles.PreloaderAddress_body_shine} />
      <Shine className={styles.PreloaderAddress_body_shine} />
      <Shine className={styles.PreloaderAddress_body_shine} />
    </div>
  </Card>
);

export default PreloaderAddress;
