import { useContext } from "react";
import { useParams } from "react-router-dom";
import { useAlert } from "react-alert";
import { Formik } from "formik";
import isEmpty from "lodash/isEmpty";

import {
  Breadcrumbs,
  Button,
  Card,
  ControlledInput,
  Spinner,
} from "src/components";

import { buttonKinds, colorNames, spinnerSizes } from "src/app-globals";
import { UserContext } from "src/contexts";
import { useContact, useUpdateContact } from "src/hooks";
import Preloader from "./Preloader";

import styles from "./styles.module.scss";
import { isValidPhoneNumber } from "src/utils/string";

const validate = (values) => {
  const errors = {};

  if (!values.firstName) {
    errors.firstName = "This field is required.";
  } else if (values.firstName.length > 50) {
    errors.firstName = "The maximum length of this field is 50 characters.";
  }

  if (!values.lastName) {
    errors.lastName = "This filed is required.";
  } else if (values.lastName.length > 50) {
    errors.lastName = "The maximum length of this field is 50 characters.";
  }

  if (!values.phoneNumber) {
    errors.phoneNumber = "This filed is required.";
  } else if (!isValidPhoneNumber(values.phoneNumber)) {
    errors.phoneNumber = "Please enter a valid phone number.";
  }

  return errors;
};

const UpdateContact = () => {
  const { user } = useContext(UserContext);
  const { contactId } = useParams();
  const alert = useAlert();

  const { isLoading: isContactLoading, contact } = useContact(contactId);
  const { isUpdating: isContactUpdating, updateContact } = useUpdateContact();

  return (
    <div className={styles.UpdateContact}>
      <Breadcrumbs
        pageTitle="Update Contact"
        pages={[
          {
            name: "Contacts",
            link: "/user/contacts",
          },
        ]}
      />
      {isContactLoading ? (
        <Preloader />
      ) : (
        <Card className={styles.UpdateContact_card}>
          <Formik
            initialValues={{
              firstName: contact.firstName,
              lastName: contact.lastName,
              phoneNumber: contact.phoneNumber,
            }}
            onSubmit={async (values, { setErrors }) => {
              const currentFormValues = {
                firstName: values.firstName,
                lastName: values.lastName,
                phoneNumber: values.phoneNumber,
                userId: user.id,
              };

              const errors = validate(values);
              if (!isEmpty(errors)) {
                setErrors(errors);
                return;
              }

              const { responseCode: updateContactResponseCode } =
                await updateContact(contactId, currentFormValues);

              const updateContactCallbacks = {
                updated: () => {
                  alert.success("Contact updated successfully.");
                },
                invalidFields: () => alert.error("Invalid fields."),
                internalError: () => alert.error("Oops, something went wrong."),
              };

              switch (updateContactResponseCode) {
                case 200:
                  updateContactCallbacks.updated();
                  break;
                case 400:
                  updateContactCallbacks.invalidFields();
                  break;
                case 500:
                  updateContactCallbacks.internalError();
                  break;
                default:
                  break;
              }
            }}
          >
            {({ errors, values, handleSubmit, setFieldValue }) => (
              <form onSubmit={handleSubmit}>
                <ControlledInput
                  name="firstName"
                  placeholder="First Name*"
                  icon="contact_mail"
                  value={values.firstName}
                  error={errors.firstName}
                  onChange={(e) => setFieldValue("firstName", e.target.value)}
                />

                <ControlledInput
                  className={styles.UpdateContact_withMargin}
                  name="lastName"
                  placeholder="Last Name*"
                  icon="contact_mail"
                  value={values.lastName}
                  error={errors.lastName}
                  onChange={(e) => setFieldValue("lastName", e.target.value)}
                />

                <ControlledInput
                  className={styles.UpdateContact_withMargin}
                  name="phoneNumber"
                  placeholder="Phone Number*"
                  icon="call"
                  value={values.phoneNumber}
                  error={errors.phoneNumber}
                  onChange={(e) => setFieldValue("phoneNumber", e.target.value)}
                />

                <div className={styles.UpdateContact_buttonGroup}>
                  <Button
                    className={styles.UpdateContact_buttonGroup_button}
                    kind={buttonKinds.SUBMIT}
                    icon="add"
                    disabled={isContactUpdating}
                    onClick={() => {}}
                  >
                    <span
                      className={styles.UpdateContact_buttonGroup_buttonText}
                    >
                      Update Contact
                      {isContactUpdating && (
                        <Spinner
                          size={spinnerSizes.XS}
                          colorName={colorNames.WHITE}
                          className={styles.UpdateContact_buttonGroup_spinner}
                        />
                      )}
                    </span>
                  </Button>
                </div>
              </form>
            )}
          </Formik>
        </Card>
      )}
    </div>
  );
};

export default UpdateContact;
