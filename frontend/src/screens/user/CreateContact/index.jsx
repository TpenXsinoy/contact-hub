import { useContext } from "react";
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
import { useCreateContact } from "src/hooks";

import { isValidPhoneNumber } from "src/utils/string";

import styles from "./styles.module.scss";

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

const CreateContact = () => {
  const { user } = useContext(UserContext);
  const alert = useAlert();

  const { isCreating: isContactCreating, createContact } = useCreateContact();

  return (
    <div className={styles.CreateContact}>
      <Breadcrumbs
        pageTitle="Create Contact"
        pages={[
          {
            name: "Contacts",
            link: "/user/contacts",
          },
        ]}
      />

      <Card className={styles.CreateContact_card}>
        <Formik
          initialValues={{
            firstName: "",
            lastName: "",
            phoneNumber: "",
          }}
          onSubmit={async (values, { setErrors, setFieldValue }) => {
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

            const { responseCode: createContactResponseCode } =
              await createContact(currentFormValues);

            const createContactCallbacks = {
              created: () => {
                alert.success("Contact created successfully.");

                // Reset form values
                setFieldValue("firstName", "");
                setFieldValue("lastName", "");
                setFieldValue("phoneNumber", "");
              },
              invalidFields: () => alert.error("Invalid fields."),
              internalError: () => alert.error("Oops, something went wrong."),
            };

            switch (createContactResponseCode) {
              case 201:
                createContactCallbacks.created();
                break;
              case 400:
                createContactCallbacks.invalidFields();
                break;
              case 500:
                createContactCallbacks.internalError();
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
                className={styles.CreateContact_withMargin}
                name="lastName"
                placeholder="Last Name*"
                icon="contact_mail"
                value={values.lastName}
                error={errors.lastName}
                onChange={(e) => setFieldValue("lastName", e.target.value)}
              />

              <ControlledInput
                className={styles.CreateContact_withMargin}
                name="phoneNumber"
                placeholder="Phone Number*"
                icon="call"
                value={values.phoneNumber}
                error={errors.phoneNumber}
                onChange={(e) => setFieldValue("phoneNumber", e.target.value)}
              />

              <div className={styles.CreateContact_buttonGroup}>
                <Button
                  className={styles.CreateContact_buttonGroup_button}
                  kind={buttonKinds.SUBMIT}
                  icon="add"
                  disabled={isContactCreating}
                  onClick={() => {}}
                >
                  <span className={styles.CreateContact_buttonGroup_buttonText}>
                    Create Contact
                    {isContactCreating && (
                      <Spinner
                        size={spinnerSizes.XS}
                        colorName={colorNames.WHITE}
                        className={styles.CreateContact_buttonGroup_spinner}
                      />
                    )}
                  </span>
                </Button>
              </div>
            </form>
          )}
        </Formik>
      </Card>
    </div>
  );
};

export default CreateContact;
