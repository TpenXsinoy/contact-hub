import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import cn from "classnames";

import {
  SweetAlert,
  ControlledInput,
  ButtonLink,
  IconButton,
  IconLink,
  Text,
  Icon,
  Breadcrumbs,
} from "src/components";

import { useAddresses, useContact, useWindowSize } from "src/hooks";

import PreloaderContact from "./Preloader/Contact";
import PreloaderAddress from "./Preloader/Address";

import { buttonTypes, colorClasses, textTypes } from "src/app-globals";
import NoResults from "src/assets/images/Misc/no-results.webp";

import styles from "./styles.module.scss";

const ViewContact = () => {
  const { contactId } = useParams();
  const [selectedAddressId, setSelectedAddressId] = useState(null);
  const [search, setSearch] = useState("");

  const { windowSize } = useWindowSize();

  const { isLoading: isContactLoading, contact } = useContact(contactId);

  const [addresses, setAddresses] = useState([]);

  useEffect(() => {
    setAddresses(!isContactLoading ? contact.addresses : []);
  }, [isContactLoading]);

  const { deleteAddress } = useAddresses(setAddresses);

  const filteredAddresses = addresses.filter((address) => {
    const fullAddress =
      `${address.addressType} ${address.street} ${address.city} ${address.state} ${address.postalCode}`.toLowerCase();
    const searchTerm = search.toLowerCase();
    return fullAddress.includes(searchTerm);
  });

  return (
    <div className={styles.ViewContact}>
      <div className={styles.ViewContact_firstRow}>
        <Breadcrumbs
          pageTitle="View Contact"
          pages={[
            {
              name: "Contacts",
              link: "/user/contacts",
            },
          ]}
        />
        <ButtonLink
          to={`/user/contacts/${contactId}/addresses/create`}
          type={buttonTypes.PRIMARY.VIOLET}
          icon="add"
        >
          Create New Address
        </ButtonLink>
      </div>

      <div className={styles.ViewContact_secondRow}>
        <Text type={textTypes.HEADING.XS}>Contact Details</Text>

        {isContactLoading ? (
          <PreloaderContact />
        ) : (
          <div className={styles.ViewContact_contactDetails}>
            <div className={styles.ViewContact_contactDetails_left}>
              <div className={styles.ViewContact_contactDetails_left_circle}>
                <Icon
                  className={styles.ViewContact_contactDetails_left_icon}
                  icon="account_box"
                />
              </div>
              <div className={styles.ViewContact_contactDetails_left_details}>
                <Text type={textTypes.HEADING.XS}>
                  {contact.firstName} {contact.lastName}
                </Text>
              </div>
            </div>

            <div className={styles.ViewContact_contactDetails_right}>
              <div className={styles.ViewContact_contactDetails_right_details}>
                {windowSize.width <= 575 && (
                  <Icon
                    className={styles.ViewContact_contactDetails_right_icon}
                    icon="share_location"
                  />
                )}
                <Text
                  type={
                    windowSize.width <= 575
                      ? textTypes.HEADING.XS
                      : textTypes.BODY.MD
                  }
                  colorClass={colorClasses.NEUTRAL["400"]}
                >
                  Total Addresses:
                </Text>

                {!isContactLoading && (
                  <Text
                    type={textTypes.HEADING.MD}
                    colorClass={colorClasses.VIOLET["400"]}
                  >
                    {addresses.length}
                  </Text>
                )}
              </div>
              {windowSize.width > 575 && (
                <Icon
                  className={styles.ViewContact_contactDetails_right_icon}
                  icon="share_location"
                />
              )}
            </div>
          </div>
        )}
      </div>

      <div className={styles.ViewContact_withMargin}>
        <Text type={textTypes.HEADING.XS}>Address List</Text>
        <ControlledInput
          className={styles.ViewContact_withMargin_searchInput}
          placeholder="Search"
          name="search"
          icon="search"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {isContactLoading ? (
        <PreloaderAddress />
      ) : (
        <>
          {filteredAddresses.length ? (
            <div className={styles.ViewContact_grid}>
              {/* Header of AddressGrid starts here */}
              <div
                className={cn(
                  styles.ViewContact_grid_addressGrid,
                  styles.ViewContact_grid_headers
                )}
              >
                <div
                  className={cn(
                    styles.ViewContact_grid_header,
                    styles.ViewContact_grid_column
                  )}
                >
                  Address Type
                </div>
                <div
                  className={cn(
                    styles.ViewContact_grid_header,
                    styles.ViewContact_grid_column
                  )}
                >
                  Street
                </div>
                <div
                  className={cn(
                    styles.ViewContact_grid_header,
                    styles.ViewContact_grid_column
                  )}
                >
                  City
                </div>
                <div
                  className={cn(
                    styles.ViewContact_grid_header,
                    styles.ViewContact_grid_column
                  )}
                >
                  State
                </div>
                <div
                  className={cn(
                    styles.ViewContact_grid_header,
                    styles.ViewContact_grid_column
                  )}
                >
                  Postal Code
                </div>
                <div
                  className={cn(
                    styles.ViewContact_grid_header,
                    styles.ViewContact_grid_column,
                    styles.ViewContact_grid_header_action
                  )}
                >
                  Actions
                </div>
                {/* Header of AddressGrid ends here */}
              </div>

              {/* Body of AddressGrid starts here */}
              {filteredAddresses.map(
                ({ id, addressType, street, city, state, postalCode }) =>
                  windowSize.width > 767 ? (
                    // For desktop view
                    <div
                      className={styles.ViewContact_grid_addressGrid}
                      key={id}
                    >
                      <div className={styles.ViewContact_grid_column}>
                        {addressType}
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        {street}
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        {city}
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        {state}
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        {postalCode}
                      </div>

                      <div className={styles.ViewContact_grid_column}>
                        <div className={styles.ViewContact_grid_buttons}>
                          <IconLink
                            to={`/user/contacts/${contactId}/addresses/${id}/update`}
                            className={styles.ViewContact_grid_editButton}
                            icon="edit"
                          />

                          <IconButton
                            className={styles.ViewContact_grid_deleteButton}
                            icon="highlight_off"
                            onClick={() => setSelectedAddressId(id)}
                          />
                        </div>
                      </div>
                    </div>
                  ) : (
                    // For mobile view
                    <details
                      className={styles.ViewContact_grid_addressGrid}
                      key={id}
                    >
                      <summary className={styles.ViewContact_grid_title}>
                        <div className={styles.ViewContact_grid_title_info}>
                          <Icon
                            icon="expand_more"
                            className={styles.ViewContact_grid_title_icon}
                          />
                          <Text type={textTypes.HEADING.XS}>{addressType}</Text>
                        </div>

                        <div className={styles.ViewContact_grid_buttons}>
                          <IconLink
                            to={`/user/contacts/${contactId}/addresses/${id}/update`}
                            className={styles.ViewContact_grid_editButton}
                            icon="edit"
                          />

                          <IconButton
                            className={styles.ViewContact_grid_deleteButton}
                            icon="highlight_off"
                            onClick={() => setSelectedAddressId(id)}
                          />
                        </div>
                      </summary>
                      <div className={styles.ViewContact_grid_column}>
                        <Text
                          type={textTypes.HEADING.XXS}
                          colorClass={colorClasses.NEUTRAL["400"]}
                        >
                          Address Type:
                        </Text>
                        <Text type={textTypes.HEADING.XXS}>{addressType}</Text>
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        <Text
                          type={textTypes.HEADING.XXS}
                          colorClass={colorClasses.NEUTRAL["400"]}
                        >
                          Street:
                        </Text>
                        <Text type={textTypes.HEADING.XXS}>{street}</Text>
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        <Text
                          type={textTypes.HEADING.XXS}
                          colorClass={colorClasses.NEUTRAL["400"]}
                        >
                          City:
                        </Text>
                        <Text type={textTypes.HEADING.XXS}>{city}</Text>
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        <Text
                          type={textTypes.HEADING.XXS}
                          colorClass={colorClasses.NEUTRAL["400"]}
                        >
                          State:
                        </Text>
                        <Text type={textTypes.HEADING.XXS}>{state}</Text>
                      </div>
                      <div className={styles.ViewContact_grid_column}>
                        <Text
                          type={textTypes.HEADING.XXS}
                          colorClass={colorClasses.NEUTRAL["400"]}
                        >
                          Postal Code:
                        </Text>
                        <Text type={textTypes.HEADING.XXS}>{postalCode}</Text>
                      </div>
                    </details>
                  )
              )}
              {/* Body of AddressGrid ends here */}
            </div>
          ) : (
            <div className={styles.ViewContact_noAddresses}>
              <img
                src={NoResults}
                alt="No Addresses"
                className={styles.ViewContact_noAddresses_image}
              />

              {!addresses.length && (
                <>
                  <Text
                    colorClass={colorClasses.RED["400"]}
                    type={textTypes.HEADING.XS}
                  >
                    CONTACT HAVE NO ADDRESS.
                  </Text>

                  <Text
                    type={textTypes.HEADING.XXS}
                    className={styles.ViewContact_noAddresses_withMargin}
                  >
                    Add one by clicking the button at the upper side of the
                    screen.
                  </Text>
                </>
              )}

              {!filteredAddresses.length && addresses.length > 0 && (
                <>
                  <Text
                    colorClass={colorClasses.RED["400"]}
                    type={textTypes.HEADING.XS}
                  >
                    ADDRESS NOT FOUND.
                  </Text>
                </>
              )}
            </div>
          )}
        </>
      )}

      <SweetAlert
        show={selectedAddressId !== null}
        title="Are you sure?"
        onConfirm={async () => {
          await deleteAddress(selectedAddressId);
          setSelectedAddressId(null);
        }}
        onCancel={() => setSelectedAddressId(null)}
      >
        This will delete the address permanently.
      </SweetAlert>
    </div>
  );
};

export default ViewContact;
