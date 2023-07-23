import cn from "classnames";
import PropTypes from "prop-types";
import ReactModal from "react-modal";

import ButtonGroup from "../Button/Group";
import Icon from "../Icon";
import Text from "../Text";
import {
  buttonKinds,
  buttonTypes,
  modalSizes,
  modalPositions,
  textTypes,
  colorClasses,
} from "src/app-globals";

import styles from "./styles.module.scss";

const Modal = ({
  className,
  size,
  children,
  position,
  handleClose,
  hasCloseButton,
  isOpen,
  actions,
  parentSelector,
  title,
  noPadding,
}) => (
  <ReactModal
    isOpen={isOpen}
    className={cn(
      className,
      styles[`Modal___${size}`],
      styles[`Modal___${position}`]
    )}
    onRequestClose={handleClose}
    parentSelector={
      parentSelector
        ? () => document.querySelector(parentSelector)
        : () => document.querySelector("body")
    }
    portalClassName={styles.Modal_portal}
    overlayClassName={styles[`Modal_overlay___${position}`]}
    bodyOpenClassName={styles.Modal_body___open}
    htmlOpenClassName={styles.Modal_html___open}
    contentLabel="Modal"
    ariaHideApp
    shouldFocusAfterRender
    shouldCloseOnOverlayClick
    shouldCloseOnEsc
    shouldReturnFocusAfterClose
  >
    <Text
      className={styles.Modal_title}
      type={textTypes.HEADING.XXS}
      colorClass={colorClasses.NEUTRAL["0"]}
    >
      {title}

      {hasCloseButton && (
        <button
          type="button"
          className={styles.Modal_close}
          onClick={handleClose}
          id="closeModal"
        >
          <Icon icon="close" className={styles.Modal_close_icon} />
        </button>
      )}
    </Text>

    <div
      className={cn(styles.Modal_content, {
        [styles.Modal_content_noPadding]: noPadding,
      })}
    >
      {children}
    </div>

    {actions && (
      <div className={styles.Modal_footer}>
        <ButtonGroup buttons={actions} />
      </div>
    )}
  </ReactModal>
);

ReactModal.setAppElement(document.getElementById("root"));

Modal.defaultProps = {
  className: null,
  size: modalSizes.SM,
  position: modalPositions.CENTER,
  handleClose: null,
  hasCloseButton: true,
  actions: null,
  parentSelector: null,
  noPadding: false,
};

Modal.propTypes = {
  className: PropTypes.string,
  size: PropTypes.oneOf([
    modalSizes.LG,
    modalSizes.MD,
    modalSizes.SM,
    modalSizes.XS,
  ]),
  position: PropTypes.oneOf([modalPositions.CENTER, modalPositions.TOP]),
  children: PropTypes.node.isRequired,
  handleClose: PropTypes.func,
  hasCloseButton: PropTypes.bool,
  isOpen: PropTypes.bool.isRequired,

  // for mapping the buttons at the bottom of the modal
  actions: PropTypes.arrayOf(
    PropTypes.shape({
      text: PropTypes.oneOfType([PropTypes.string, PropTypes.element])
        .isRequired,
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
    })
  ),

  parentSelector: PropTypes.string,
  title: PropTypes.string.isRequired,
  noPadding: PropTypes.bool,
};

export default Modal;
