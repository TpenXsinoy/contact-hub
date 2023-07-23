import { useState, useEffect } from "react";
import { useAlert } from "react-alert";

import { ContactsService } from "src/services";

const useContacts = () => {
  const alert = useAlert();
  const [isLoading, setIsLoading] = useState(true);
  const [contacts, setContacts] = useState([]);

  const deleteContact = async (contactId) => {
    const { status: deleteContactStatus } = await ContactsService.delete(
      contactId
    );

    if (deleteContactStatus === 200) {
      alert.success("Contact deleted.");

      setContacts((prevContacts) =>
        prevContacts.filter((prevContact) => prevContact.id !== contactId)
      );
    } else {
      alert.error("Oops, something went wrong.");
    }
  };

  useEffect(() => {
    const getContacts = async () => {
      const { data: getContactsResponse } = await ContactsService.list();

      if (getContactsResponse) {
        setContacts(getContactsResponse);
      }

      setIsLoading(false);
    };

    getContacts();
  }, []);

  return { isLoading, contacts, deleteContact };
};

export default useContacts;
