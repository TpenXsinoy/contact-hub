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

import { useAddress, useUpdateAddress } from "src/hooks";
import Preloader from "./Preloader";

import styles from "./styles.module.scss";

const validate = (values) => {
  const errors = {};

  if (!values.addressType) {
    errors.addressType = "This field is required.";
  } else if (values.addressType.length > 50) {
    errors.addressType = "The maximum length of this field is 50 characters.";
  } else if (values.addressType.length < 3) {
    errors.addressType =
      "The minimum length of this field is three characters.";
  }

  if (values.street.length > 100) {
    errors.street = "The maximum length of this field is 100 characters.";
  }

  if (!values.city) {
    errors.city = "This filed is required.";
  } else if (values.city.length > 50) {
    errors.city = "The maximum length of this field is 50 characters.";
  } else if (values.city.length < 2) {
    errors.city = "The minimum length of this field is two characters.";
  }

  if (values.state.length > 50) {
    errors.state = "The maximum length of this field is 50 characters.";
  }

  if (!values.postalCode) {
    errors.postalCode = "This filed is required.";
  } else if (values.postalCode.length > 50) {
    errors.postalCode = "The maximum length of this field is 50 characters.";
  } else if (values.postalCode.length < 2) {
    errors.postalCode = "The minimum length of this field is two characters.";
  }

  return errors;
};

const UpdateAddress = () => {
  const { contactId, addressId } = useParams();
  const alert = useAlert();

  const { isLoading: isAddressLoading, address } = useAddress(addressId);
  const { isUpdating: isAddressUpdating, updateAddress } = useUpdateAddress();

  return (
    <div className={styles.UpdateAddress}>
      <Breadcrumbs
        pageTitle="Update Address"
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
      {isAddressLoading ? (
        <Preloader />
      ) : (
        <Card className={styles.UpdateAddress_card}>
          <Formik
            initialValues={{
              addressType: address.addressType,
              street: address.street,
              city: address.city,
              state: address.state,
              postalCode: address.postalCode,
            }}
            onSubmit={async (values, { setErrors }) => {
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

              const { responseCode: updateAddressResponseCode } =
                await updateAddress(addressId, currentFormValues);

              const updateAddressCallbacks = {
                updated: () => {
                  alert.success("Address updated successfully.");
                },
                invalidFields: () => alert.error("Invalid fields."),
                internalError: () => alert.error("Oops, something went wrong."),
              };

              switch (updateAddressResponseCode) {
                case 200:
                  updateAddressCallbacks.updated();
                  break;
                case 400:
                  updateAddressCallbacks.invalidFields();
                  break;
                case 500:
                  updateAddressCallbacks.internalError();
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
                  className={styles.UpdateAddress_withMargin}
                  name="street"
                  placeholder="Street"
                  icon="add_road"
                  value={values.street}
                  error={errors.street}
                  onChange={(e) => setFieldValue("street", e.target.value)}
                />

                <ControlledInput
                  className={styles.UpdateAddress_withMargin}
                  name="city"
                  placeholder="City*"
                  icon="location_city"
                  value={values.city}
                  error={errors.city}
                  onChange={(e) => setFieldValue("city", e.target.value)}
                />

                <ControlledInput
                  className={styles.UpdateAddress_withMargin}
                  name="state"
                  placeholder="State"
                  icon="explore"
                  value={values.state}
                  error={errors.state}
                  onChange={(e) => setFieldValue("state", e.target.value)}
                />

                <ControlledInput
                  className={styles.UpdateAddress_withMargin}
                  name="postalCode"
                  placeholder="Postal Code*"
                  icon="pin"
                  value={values.postalCode}
                  error={errors.postalCode}
                  onChange={(e) => setFieldValue("postalCode", e.target.value)}
                />

                <div className={styles.UpdateAddress_buttonGroup}>
                  <Button
                    className={styles.UpdateAddress_buttonGroup_button}
                    kind={buttonKinds.SUBMIT}
                    icon="add"
                    disabled={isAddressUpdating}
                    onClick={() => {}}
                  >
                    <span
                      className={styles.UpdateAddress_buttonGroup_buttonText}
                    >
                      Update Address
                      {isAddressUpdating && (
                        <Spinner
                          size={spinnerSizes.XS}
                          colorName={colorNames.WHITE}
                          className={styles.UpdateAddress_buttonGroup_spinner}
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

export default UpdateAddress;
