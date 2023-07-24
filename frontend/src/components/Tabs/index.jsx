import cn from "classnames";
import PropTypes from "prop-types";

import TabButton from "./TabButton";
import TabLink from "./TabLink";
import { tabKinds, tabTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const Tabs = ({ type, activeTab, tabs, className, tabClassName }) => (
  <div className={cn(className, styles[`Tabs___${type}`])}>
    {tabs?.map(({ name, value, kind, action, closeAction, id }) =>
      kind === tabKinds.BUTTON ? (
        <TabButton
          key={value}
          className={cn(styles.Tabs_button, tabClassName)}
          onClick={action}
          type={type}
          active={activeTab === value}
          id={id}
          closeAction={
            type === tabTypes.HORIZONTAL.SM && closeAction ? closeAction : null
          }
        >
          {name}
        </TabButton>
      ) : (
        <TabLink
          key={value}
          className={cn(styles.Tabs_link, tabClassName)}
          to={action}
          type={type}
          active={activeTab === value}
          id={id}
          closeAction={
            type === tabTypes.HORIZONTAL.SM && closeAction ? closeAction : null
          }
        >
          {name}
        </TabLink>
      )
    )}
  </div>
);

Tabs.defaultProps = {
  className: null,
  tabClassName: null,
  activeTab: null,
};

Tabs.propTypes = {
  className: PropTypes.string,
  tabClassName: PropTypes.string,
  type: PropTypes.oneOf([
    tabTypes.HORIZONTAL.LG,
    tabTypes.HORIZONTAL.SM,
    tabTypes.VERTICAL.LG,
    tabTypes.VERTICAL.SM,
  ]).isRequired,
  tabs: PropTypes.arrayOf(
    PropTypes.shape({
      name: PropTypes.string.isRequired,
      value: PropTypes.oneOfType([PropTypes.string, PropTypes.number])
        .isRequired,
      kind: PropTypes.oneOf([tabKinds.BUTTON, tabKinds.LINK]).isRequired,
      // if type is BUTTON -> func, else if type is LINK -> string
      action: PropTypes.oneOfType([PropTypes.func, PropTypes.string])
        .isRequired,
      closeAction: PropTypes.func,
      id: PropTypes.string,
    })
  ).isRequired,
  activeTab: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
};

export default Tabs;
