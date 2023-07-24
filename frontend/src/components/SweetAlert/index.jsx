import PropTypes from "prop-types";
import Swal from "react-bootstrap-sweetalert";

import Button from "../Button";
import { buttonTypes, sweetAlertTypes } from "src/app-globals";

import styles from "./styles.module.scss";

const SweetAlert = ({
  show,
  type,
  title,
  children,
  confirmBtnText,
  cancelBtnText,
  buttonsDisabled,
  onConfirm,
  onCancel,
}) => (
  <Swal
    show={show}
    type={type}
    title={title}
    customButtons={
      <>
        <Button
          type={buttonTypes.PRIMARY.RED}
          className={styles.SweetAlert_button}
          onClick={onCancel}
          disabled={buttonsDisabled}
        >
          {cancelBtnText}
        </Button>
        <Button
          type={buttonTypes.PRIMARY.GREEN}
          className={styles.SweetAlert_button}
          onClick={onConfirm}
          disabled={buttonsDisabled}
        >
          {confirmBtnText}
        </Button>
      </>
    }
    onConfirm={() => {}}
  >
    {children}
  </Swal>
);

SweetAlert.defaultProps = {
  show: false,
  type: sweetAlertTypes.WARNING,
  children: null,
  confirmBtnText: "Yes",
  cancelBtnText: "No",
  buttonsDisabled: false,
  onCancel: () => {},
};

SweetAlert.propTypes = {
  show: PropTypes.bool,
  type: PropTypes.oneOf([
    sweetAlertTypes.INFO,
    sweetAlertTypes.SUCCESS,
    sweetAlertTypes.WARNING,
    sweetAlertTypes.DANGER,
    sweetAlertTypes.CONTROLLED,
  ]),
  title: PropTypes.string.isRequired,
  children: PropTypes.any,
  confirmBtnText: PropTypes.string,
  cancelBtnText: PropTypes.string,
  buttonsDisabled: PropTypes.bool,
  onConfirm: PropTypes.func.isRequired,
  onCancel: PropTypes.func,
};

export default SweetAlert;
