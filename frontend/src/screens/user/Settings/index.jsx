import React from "react";
import { Switch, Redirect, Route, useParams } from "react-router-dom";

import { Card, Tabs, Text } from "src/components";
import { colorClasses, tabKinds, tabTypes, textTypes } from "src/app-globals";
import settingsTabs from "./constants/settingsTabs";
import AccountInformation from "./AccountInformation";
import ChangePassword from "./ChangePassword";

import styles from "./styles.module.scss";

const Settings = () => {
  const { activeTab } = useParams();
  return (
    <div className={styles.Settings}>
      <div>
        <Text
          className={styles.Settings_title}
          type={textTypes.HEADING.XS}
          colorClass={colorClasses.NEUTRAL["300"]}
        >
          Settings
        </Text>
        <Tabs
          type={tabTypes.VERTICAL.LG}
          tabs={[
            {
              name: settingsTabs.ACCOUNT_INFORMATION.name,
              value: settingsTabs.ACCOUNT_INFORMATION.value,
              kind: tabKinds.LINK,
              action: `/user/settings/${settingsTabs.ACCOUNT_INFORMATION.value}`,
            },
            {
              name: settingsTabs.CHANGE_PASSWORD.name,
              value: settingsTabs.CHANGE_PASSWORD.value,
              kind: tabKinds.LINK,
              action: `/user/settings/${settingsTabs.CHANGE_PASSWORD.value}`,
            },
          ]}
          activeTab={activeTab}
        />
      </div>

      <Card className={styles.Settings_card}>
        <React.Suspense fallback={<div>loading</div>}>
          <Switch>
            <Route
              path={`/user/settings/${settingsTabs.ACCOUNT_INFORMATION.value}`}
              name="Settings - Account Information"
              render={(props) => <AccountInformation {...props} />}
            />

            <Route
              path={`/user/settings/${settingsTabs.CHANGE_PASSWORD.value}`}
              name="Settings - Change Password"
              render={(props) => <ChangePassword {...props} />}
            />

            <Redirect to="/page-not-found" />
          </Switch>
        </React.Suspense>
      </Card>
    </div>
  );
};

export default Settings;
