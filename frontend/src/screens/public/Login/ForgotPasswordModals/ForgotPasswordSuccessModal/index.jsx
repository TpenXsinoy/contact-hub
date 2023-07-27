import PropTypes from "prop-types";

import { buttonTypes } from "src/app-globals";
import { Modal, Text } from "src/components";

const ForgotPasswordSuccessModal = ({ isOpen, handleClose }) => (
  <Modal
    isOpen={isOpen}
    handleClose={handleClose}
    title="Success"
    actions={[
      {
        text: "Finish",
        type: buttonTypes.PRIMARY.GREEN,
        onClick: handleClose,
      },
    ]}
  >
    <Text>
      You have successfully changed your password. You can now login with that
      new password.
    </Text>
  </Modal>
);

ForgotPasswordSuccessModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  handleClose: PropTypes.func.isRequired,
};

export default ForgotPasswordSuccessModal;
