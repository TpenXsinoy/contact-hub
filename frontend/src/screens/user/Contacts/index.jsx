import { useState, useContext } from "react";
import cn from "classnames";

import {
  SweetAlert,
  ControlledInput,
  ButtonLink,
  IconButton,
  IconLink,
  Text,
  Icon,
} from "src/components";

import { buttonTypes, colorClasses, textTypes } from "src/app-globals";

import { UserContext } from "src/contexts";
import { useWindowSize } from "src/hooks";
import { useContacts } from "src/hooks";

import PreloaderContact from "./Preloader";
import NoResults from "src/assets/images/Misc/no-results.webp";

import styles from "./styles.module.scss";

const Contacts = () => {
  const { user } = useContext(UserContext);
  const [selectedContactId, setSelectedContactId] = useState(null);
  const [search, setSearch] = useState("");

  const { windowSize } = useWindowSize();

  const {
    isLoading: isContactListLoading,
    contacts,
    deleteContact,
  } = useContacts();

  const filteredContacts = contacts.filter((contact) => {
    const fullName = `${contact.firstName} ${contact.lastName}`.toLowerCase();
    const searchTerm = search.toLowerCase();
    return fullName.includes(searchTerm);
  });

  return (
    <div className={styles.Contacts}>
      <div className={styles.Contacts_firstRow}>
        <Text
          type={textTypes.HEADING.XXS}
          colorClass={colorClasses.NEUTRAL["400"]}
        >
          Hello! {user.firstName}
        </Text>

        <ButtonLink
          to={`/user/contacts/create`}
          type={buttonTypes.PRIMARY.VIOLET}
          icon="add"
        >
          Create New Contact
        </ButtonLink>
      </div>

      <div className={styles.Contacts_secondRow}>
        <Text type={textTypes.HEADING.XS}>Account Details</Text>

        <div className={styles.Contacts_userDetails}>
          <div className={styles.Contacts_userDetails_left}>
            <div className={styles.Contacts_userDetails_left_circle}>
              <Icon
                className={styles.Contacts_userDetails_left_icon}
                icon="person"
              />
            </div>

            <div className={styles.Contacts_userDetails_left_details}>
              <Text type={textTypes.HEADING.XS}>
                {user.firstName} {user.lastName}
              </Text>

              <div className={styles.Contacts_userDetails_left_details_subinfo}>
                <Icon icon="email" />

                <Text
                  type={textTypes.BODY.MD}
                  colorClass={colorClasses.NEUTRAL["400"]}
                >
                  {user.email}
                </Text>
              </div>
            </div>
          </div>

          <div className={styles.Contacts_userDetails_right}>
            <div className={styles.Contacts_userDetails_right_details}>
              {windowSize.width <= 575 && (
                <Icon
                  className={styles.Contacts_userDetails_right_icon}
                  icon="contacts"
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
                Total Contacts:
              </Text>

              {!isContactListLoading && (
                <Text
                  type={textTypes.HEADING.MD}
                  colorClass={colorClasses.VIOLET["400"]}
                >
                  {contacts.length}
                </Text>
              )}
            </div>

            {windowSize.width > 575 && (
              <Icon
                className={styles.Contacts_userDetails_right_icon}
                icon="contacts"
              />
            )}
          </div>
        </div>
      </div>

      <div className={styles.Contacts_withMargin}>
        <Text type={textTypes.HEADING.XS}>Contact List</Text>

        <ControlledInput
          className={styles.Contacts_withMargin_searchInput}
          placeholder="Search"
          name="search"
          icon="search"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {isContactListLoading ? (
        <PreloaderContact />
      ) : (
        <>
          {filteredContacts.length ? (
            <div className={styles.Contacts_grid}>
              {/* Header of ContactGrid starts here */}
              <div
                className={cn(
                  styles.Contacts_grid_contactGrid,
                  styles.Contacts_grid_headers
                )}
              >
                <div
                  className={cn(
                    styles.Contacts_grid_header,
                    styles.Contacts_grid_column
                  )}
                >
                  First Name
                </div>

                <div
                  className={cn(
                    styles.Contacts_grid_header,
                    styles.Contacts_grid_column
                  )}
                >
                  Last Name
                </div>

                <div
                  className={cn(
                    styles.Contacts_grid_header,
                    styles.Contacts_grid_column
                  )}
                >
                  Contact Number
                </div>

                <div
                  className={cn(
                    styles.Contacts_grid_header,
                    styles.Contacts_grid_column,
                    styles.Contacts_grid_header_action
                  )}
                >
                  Actions
                </div>
                {/* Header of ContactGrid ends here */}
              </div>

              {/* Body of ContactGrid starts here */}
              {filteredContacts.map(
                ({ id, firstName, lastName, phoneNumber }) =>
                  windowSize.width > 767 ? (
                    // Desktop View
                    <div className={styles.Contacts_grid_contactGrid} key={id}>
                      <div className={styles.Contacts_grid_column}>
                        {firstName}
                      </div>

                      <div className={styles.Contacts_grid_column}>
                        {lastName}
                      </div>

                      <div className={styles.Contacts_grid_column}>
                        {phoneNumber}
                      </div>

                      <div className={styles.Contacts_grid_column}>
                        <div className={styles.Contacts_grid_buttons}>
                          <IconLink
                            to={`/user/contacts/${id}`}
                            className={styles.Contacts_grid_viewButton}
                            icon="visibility"
                          />

                          <IconLink
                            to={`/user/contacts/${id}/update`}
                            className={styles.Contacts_grid_editButton}
                            icon="edit"
                          />

                          <IconButton
                            className={styles.Contacts_grid_deleteButton}
                            icon="highlight_off"
                            onClick={() => setSelectedContactId(id)}
                          />
                        </div>
                      </div>
                    </div>
                  ) : (
                    // Mobile View
                    <details
                      className={styles.Contacts_grid_contactGrid}
                      key={id}
                    >
                      <summary className={styles.Contacts_grid_title}>
                        <div className={styles.Contacts_grid_title_info}>
                          <Icon
                            icon="expand_more"
                            className={styles.Contacts_grid_title_icon}
                          />

                          <Text type={textTypes.HEADING.XS}>
                            {firstName} {lastName}
                          </Text>
                        </div>

                        <div className={styles.Contacts_grid_buttons}>
                          <IconLink
                            to={`/user/contacts/${id}`}
                            className={styles.Contacts_grid_viewButton}
                            icon="visibility"
                          />

                          <IconLink
                            to={`/user/contacts/${id}/update`}
                            className={styles.Contacts_grid_editButton}
                            icon="edit"
                          />

                          <IconButton
                            className={styles.Contacts_grid_deleteButton}
                            icon="highlight_off"
                            onClick={() => setSelectedContactId(id)}
                          />
                        </div>
                      </summary>

                      <div className={styles.Contacts_grid_column}>
                        <Text
                          type={textTypes.HEADING.XXS}
                          colorClass={colorClasses.NEUTRAL["400"]}
                        >
                          Contact Number:
                        </Text>

                        <Text type={textTypes.HEADING.XXS}>{phoneNumber}</Text>
                      </div>
                    </details>
                  )
              )}
              {/* Body of ContactGrid ends here */}
            </div>
          ) : (
            <div className={styles.Contacts_noContacts}>
              <img
                src={NoResults}
                alt="No Contacts"
                className={styles.Contacts_noContacts_image}
              />

              {!contacts.length && (
                <>
                  <Text
                    colorClass={colorClasses.RED["400"]}
                    type={textTypes.HEADING.XS}
                  >
                    YOU HAVE NO CONTACTS.
                  </Text>

                  <Text
                    type={textTypes.HEADING.XXS}
                    className={styles.Contacts_noContacts_withMargin}
                  >
                    Add one by clicking the button at the upper side of the
                    screen.
                  </Text>
                </>
              )}

              {!filteredContacts.length && contacts.length > 0 && (
                <>
                  <Text
                    colorClass={colorClasses.RED["400"]}
                    type={textTypes.HEADING.XS}
                  >
                    CONTACT NOT FOUND.
                  </Text>
                </>
              )}
            </div>
          )}
        </>
      )}

      <SweetAlert
        show={selectedContactId !== null}
        title="Are you sure?"
        onConfirm={async () => {
          await deleteContact(selectedContactId);
          setSelectedContactId(null);
        }}
        onCancel={() => setSelectedContactId(null)}
      >
        This will delete the contact permanently.
      </SweetAlert>
    </div>
  );
};

export default Contacts;
