import cn from "classnames";
import PropTypes from "prop-types";

import Icon from "../../Icon";
import Text from "../../Text";
import {
  colorClasses,
  inputKinds,
  inputTypes,
  textTypes,
} from "src/app-globals";

import styles from "../input.module.scss";

const ControlledInput = ({
  type,
  kind,
  className,
  placeholder,
  error,
  success,
  name,
  label,
  disabled,
  value,
  onChange,
  onBlur,
  icon,
  maxLength,
  autoComplete,
  helperText,
  step,
  tabIndex,
}) => (
  <div className={cn(className, styles.Input_container)}>
    {label && type === inputTypes.SLIM && (
      <Text
        className={styles.Input___slim_label}
        type={textTypes.BODY.MD}
        colorClass={colorClasses.NEUTRAL["700"]}
      >
        {label}
      </Text>
    )}

    <input
      type={kind}
      placeholder={type === inputTypes.SLIM && placeholder ? placeholder : null}
      className={cn(styles[`Input___${type}`], {
        [styles[`Input___${type}___icon`]]: icon !== null,
        [styles.Input___success]: success !== null,
        [styles.Input___error]: error !== null,
        [styles.Input___disabled]: disabled,
      })}
      id={name}
      name={name}
      disabled={disabled}
      maxLength={maxLength}
      autoComplete={autoComplete}
      value={value || ""}
      onChange={onChange}
      onBlur={onBlur}
      step={step}
      tabIndex={tabIndex}
    />

    {placeholder && type === inputTypes.FORM && (
      <Text
        className={cn(styles.Input___form_label, {
          [styles.Input___form_label___active]: value !== "",
        })}
        type={textTypes.BODY.MD}
        colorClass={colorClasses.NEUTRAL["500"]}
      >
        {placeholder}
      </Text>
    )}

    {icon && <Icon className={styles[`Input___${type}_icon`]} icon={icon} />}

    {(helperText || success || error) && (
      <div className={styles.Input_helperTextContainer}>
        {helperText && (
          <Text
            type={textTypes.BODY.XS}
            colorClass={colorClasses.NEUTRAL["500"]}
          >
            {helperText}
          </Text>
        )}

        {error && (
          <Text type={textTypes.BODY.XS} colorClass={colorClasses.RED["400"]}>
            {error}
          </Text>
        )}

        {success && (
          <Text type={textTypes.BODY.XS} colorClass={colorClasses.GREEN["400"]}>
            {success}
          </Text>
        )}
      </div>
    )}
  </div>
);

ControlledInput.defaultProps = {
  type: inputTypes.FORM,
  kind: inputKinds.TEXT,
  className: null,
  placeholder: null,
  error: null,
  success: null,
  label: null,
  disabled: false,
  value: "",
  onChange: () => {},
  onBlur: () => {},
  maxLength: null,
  autoComplete: "off",
  helperText: null,
  icon: null,
  step: null,
  tabIndex: null,
};

ControlledInput.propTypes = {
  type: PropTypes.oneOf([inputTypes.FORM, inputTypes.SLIM]),
  kind: PropTypes.oneOf([
    inputKinds.NUMBER,
    inputKinds.PASSWORD,
    inputKinds.TEXT,
  ]),
  className: PropTypes.string,
  placeholder: PropTypes.string,
  error: PropTypes.string,
  success: PropTypes.string,
  name: PropTypes.string.isRequired,
  label: PropTypes.string,
  disabled: PropTypes.bool,
  value: PropTypes.string,
  onChange: PropTypes.func,
  onBlur: PropTypes.func,
  icon: PropTypes.string,
  maxLength: PropTypes.number,
  autoComplete: PropTypes.string,
  helperText: PropTypes.string,
  step: PropTypes.number,
  tabIndex: PropTypes.number,
};

export default ControlledInput;
