import { Card, Shine } from "src/components";

import styles from "./styles.module.scss";

const Preloader = () => (
  <Card className={styles.Preloader}>
    <Shine className={styles.Preloader_shine} />
  </Card>
);

export default Preloader;
