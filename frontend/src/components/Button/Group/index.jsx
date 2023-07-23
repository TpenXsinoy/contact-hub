import PropTypes from "prop-types";
import cn from "classnames";

import Button from "..";
import {
  buttonKinds,
  buttonTypes,
  buttonGroupDirections,
} from "src/app-globals";

import styles from "./styles.module.scss";

const ButtonGroup = ({ className, buttons, direction }) => (
  <div className={cn(className, styles[`ButtonGroup___${direction}`])}>
    {buttons &&
      buttons.map(
        ({
          onClick,
          text,
          kind,
          type: buttonType,
          disabled,
          id,
          dataTest,
          icon,
          iconPosition,
        }) => (
          <Button
            dataTest={dataTest}
            key={text}
            kind={kind || buttonKinds.BUTTON}
            className={styles[`ButtonGroup___${direction}_button`]}
            type={buttonType}
            onClick={onClick}
            disabled={disabled}
            id={id}
            icon={icon}
            iconPosition={iconPosition}
          >
            {text}
          </Button>
        )
      )}
  </div>
);

ButtonGroup.defaultProps = {
  className: null,
  direction: buttonGroupDirections.HORIZONTAL,
};

ButtonGroup.propTypes = {
  className: PropTypes.string,
  buttons: PropTypes.arrayOf(
    PropTypes.shape({
      text: PropTypes.oneOfType([PropTypes.string, PropTypes.element])
        .isRequired,
      id: PropTypes.string,
      type: PropTypes.oneOf([
        buttonTypes.PRIMARY.VIOLET,
        buttonTypes.PRIMARY.BLUE,
        buttonTypes.PRIMARY.RED,
        buttonTypes.PRIMARY.GREEN,
        buttonTypes.PRIMARY.YELLOW,
        buttonTypes.SECONDARY.VIOLET,
        buttonTypes.SECONDARY.BLUE,
        buttonTypes.SECONDARY.RED,
        buttonTypes.SECONDARY.GREEN,
        buttonTypes.SECONDARY.YELLOW,
        buttonTypes.TEXT.VIOLET,
        buttonTypes.TEXT.BLUE,
        buttonTypes.TEXT.RED,
        buttonTypes.TEXT.GREEN,
        buttonTypes.TEXT.YELLOW,
        buttonTypes.TERTIARY,
      ]),
      kind: PropTypes.oneOf([
        buttonKinds.BUTTON,
        buttonKinds.SUBMIT,
        buttonKinds.RESET,
      ]),
      disabled: PropTypes.bool,
      onClick: PropTypes.func.isRequired,
      dataTest: PropTypes.string,
      icon: PropTypes.string,
      iconPosition: PropTypes.oneOf(["left", "right"]),
    })
  ).isRequired,
  direction: PropTypes.oneOf([
    buttonGroupDirections.HORIZONTAL,
    buttonGroupDirections.VERTICAL,
  ]),
};

export default ButtonGroup;
