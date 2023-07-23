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

import { useCreateAddress } from "src/hooks";
import { buttonKinds, colorNames, spinnerSizes } from "src/app-globals";

import styles from "./styles.module.scss";

const validate = (values) => {
  const errors = {};

  if (!values.addressType) {
    errors.addressType = "This field is required.";
  } else if (values.addressType.length > 50) {
    errors.addressType = "The maximum length of this field is 50 characters.";
  }

  if (!values.street) {
    errors.street = "This filed is required.";
  } else if (values.street.length > 100) {
    errors.street = "The maximum length of this field is 100 characters.";
  }

  if (!values.city) {
    errors.city = "This filed is required.";
  } else if (values.city.length > 50) {
    errors.city = "The maximum length of this field is 50 characters.";
  }

  if (!values.state) {
    errors.state = "This filed is required.";
  } else if (values.state.length > 50) {
    errors.state = "The maximum length of this field is 50 characters.";
  }

  if (!values.postalCode) {
    errors.postalCode = "This filed is required.";
  } else if (values.postalCode.length > 50) {
    errors.postalCode = "The maximum length of this field is 50 characters.";
  }

  return errors;
};

const CreateAddress = () => {
  const { contactId } = useParams();
  const alert = useAlert();

  const { isCreating: isAddressCreating, createAddress } = useCreateAddress();

  return (
    <div className={styles.CreateAddress}>
      <Breadcrumbs
        pageTitle="Create Address"
        pages={[
          {
            name: "Contacts",
            link: "/user/contacts",
          },
          {
            name: "View Contact",
            link: `/user/contacts/${contactId}`,
          },
        ]}
      />

      <Card className={styles.CreateAddress_card}>
        <Formik
          initialValues={{
            addressType: "",
            street: "",
            city: "",
            state: "",
            postalCode: "",
          }}
          onSubmit={async (values, { setErrors, setFieldValue }) => {
            const currentFormValues = {
              addressType: values.addressType,
              street: values.street,
              city: values.city,
              state: values.state,
              postalCode: values.postalCode,
              contactId: contactId,
            };

            const errors = validate(values);
            if (!isEmpty(errors)) {
              setErrors(errors);
              return;
            }

            const { responseCode: createAddressResponseCode } =
              await createAddress(currentFormValues);

            const createAddressCallbacks = {
              created: () => {
                alert.success("Address created successfully.");

                // Reset form values
                setFieldValue("addressType", "");
                setFieldValue("street", "");
                setFieldValue("city", "");
                setFieldValue("state", "");
                setFieldValue("postalCode", "");
              },
              invalidFields: () => alert.error("Invalid fields."),
              internalError: () => alert.error("Oops, something went wrong."),
            };

            switch (createAddressResponseCode) {
              case 201:
                createAddressCallbacks.created();
                break;
              case 400:
                createAddressCallbacks.invalidFields();
                break;
              case 500:
                createAddressCallbacks.internalError();
                break;
              default:
                break;
            }
          }}
        >
          {({ errors, values, handleSubmit, setFieldValue }) => (
            <form onSubmit={handleSubmit}>
              <ControlledInput
                name="addressType"
                placeholder="Address Type*"
                icon="map"
                value={values.addressType}
                error={errors.addressType}
                onChange={(e) => setFieldValue("addressType", e.target.value)}
              />

              <ControlledInput
                className={styles.CreateAddress_withMargin}
                name="street"
                placeholder="Street*"
                icon="add_road"
                value={values.street}
                error={errors.street}
                onChange={(e) => setFieldValue("street", e.target.value)}
              />

              <ControlledInput
                className={styles.CreateAddress_withMargin}
                name="city"
                placeholder="City*"
                icon="location_city"
                value={values.city}
                error={errors.city}
                onChange={(e) => setFieldValue("city", e.target.value)}
              />

              <ControlledInput
                className={styles.CreateAddress_withMargin}
                name="state"
                placeholder="State*"
                icon="explore"
                value={values.state}
                error={errors.state}
                onChange={(e) => setFieldValue("state", e.target.value)}
              />

              <ControlledInput
                className={styles.CreateAddress_withMargin}
                name="postalCode"
                placeholder="Postal Code*"
                icon="pin"
                value={values.postalCode}
                error={errors.postalCode}
                onChange={(e) => setFieldValue("postalCode", e.target.value)}
              />

              <div className={styles.CreateAddress_buttonGroup}>
                <Button
                  className={styles.CreateAddress_buttonGroup_button}
                  kind={buttonKinds.SUBMIT}
                  icon="add"
                  disabled={isAddressCreating}
                  onClick={() => {}}
                >
                  <span className={styles.CreateAddress_buttonGroup_buttonText}>
                    Create Address
                    {isAddressCreating && (
                      <Spinner
                        size={spinnerSizes.XS}
                        colorName={colorNames.WHITE}
                        className={styles.CreateAddress_buttonGroup_spinner}
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

export default CreateAddress;
