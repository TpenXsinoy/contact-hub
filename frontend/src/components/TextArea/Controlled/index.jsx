import cn from "classnames";
import PropTypes from "prop-types";

import Icon from "../../Icon";
import Text from "../../Text";
import { colorClasses, textTypes } from "src/app-globals";

import styles from "../styles.module.scss";

const ControlledTextArea = ({
  className,
  inputClassName,
  placeholder,
  error,
  success,
  name,
  disabled,
  value,
  icon,
  helperText,
  onChange,
  withValue,
  withStyles,
}) => (
  <div className={cn(className, styles.TextArea_container)}>
    <textarea
      className={cn(
        inputClassName,
        {
          [styles.TextArea_input]: withStyles,
        },
        {
          [styles.TextArea___icon]: icon !== null,
          [styles.TextArea___success]: success !== null,
          [styles.TextArea___error]: error !== null,
        }
      )}
      name={name}
      value={withValue ? value : undefined}
      disabled={disabled}
      onChange={onChange}
    />

    {placeholder && (
      <Text
        className={cn(styles.TextArea_input_placeholder, {
          [styles.TextArea_input_placeholder___active]: value !== "",
        })}
        type={textTypes.BODY.MD}
        colorClass={colorClasses.NEUTRAL["500"]}
      >
        {placeholder}
      </Text>
    )}

    {icon && <Icon className={styles.TextArea_icon} icon={icon} />}

    {(helperText || success || error) && (
      <div className={styles.TextArea_helperTextContainer}>
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

ControlledTextArea.defaultProps = {
  className: null,
  inputClassName: null,
  placeholder: null,
  error: null,
  success: null,
  disabled: false,
  value: "",
  helperText: null,
  icon: null,
  withValue: true,
  withStyles: true,
};

ControlledTextArea.propTypes = {
  className: PropTypes.string,
  inputClassName: PropTypes.string,
  placeholder: PropTypes.string,
  error: PropTypes.string,
  success: PropTypes.string,
  name: PropTypes.string.isRequired,
  disabled: PropTypes.bool,
  value: PropTypes.string,
  onChange: PropTypes.func.isRequired,
  icon: PropTypes.string,
  helperText: PropTypes.string,
  withValue: PropTypes.bool,
  withStyles: PropTypes.bool,
};

export default ControlledTextArea;
