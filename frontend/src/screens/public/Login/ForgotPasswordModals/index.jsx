import { useState } from "react";
import PropTypes from "prop-types";

import { forgotPasswordSteps } from "./constants";

import InputEmailModal from "./InputEmailModal";
import InputConfirmationCodeModal from "./InputConfirmationCodeModal";
import ChangePasswordModal from "./ChangePasswordModal";
import ForgotPasswordSuccessModal from "./ForgotPasswordSuccessModal";

const ForgotPasswordModals = ({ isOpen, handleClose }) => {
  const [step, setStep] = useState(forgotPasswordSteps.INPUT_USERNAME_EMAIL);
  const [retrievedUser, setRetrievedUser] = useState(null);

  return (
    <>
      {step === forgotPasswordSteps.INPUT_USERNAME_EMAIL && (
        <InputEmailModal
          isOpen={isOpen}
          handleClose={handleClose}
          handleSuccess={({ user }) => {
            setRetrievedUser(user);
            setStep(forgotPasswordSteps.INPUT_CONFIRMATION_CODE);
          }}
        />
      )}

      {step === forgotPasswordSteps.INPUT_CONFIRMATION_CODE && (
        <InputConfirmationCodeModal
          isOpen={isOpen}
          handleClose={handleClose}
          user={retrievedUser}
          handleSuccess={() => {
            setStep(forgotPasswordSteps.CHANGE_PASSWORD);
          }}
        />
      )}

      {step === forgotPasswordSteps.CHANGE_PASSWORD && (
        <ChangePasswordModal
          isOpen={isOpen}
          handleClose={handleClose}
          user={retrievedUser}
          handleSuccess={() => {
            setStep(forgotPasswordSteps.FORGOT_PASSWORD_SUCCESS);
          }}
        />
      )}

      {step === forgotPasswordSteps.FORGOT_PASSWORD_SUCCESS && (
        <ForgotPasswordSuccessModal isOpen={isOpen} handleClose={handleClose} />
      )}
    </>
  );
};

ForgotPasswordModals.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  handleClose: PropTypes.func.isRequired,
};

export default ForgotPasswordModals;
