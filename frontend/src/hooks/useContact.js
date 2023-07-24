import { useState, useEffect } from "react";

import { ContactsService } from "src/services";

const useContact = (contactId) => {
  const [isLoading, setIsLoading] = useState(true);
  const [contact, setContact] = useState(null);

  useEffect(() => {
    const getContact = async () => {
      const { data: getContactResponse } = await ContactsService.retrieve(
        contactId
      );

      if (getContactResponse) {
        setContact(getContactResponse);
      }

      setIsLoading(false);
    };

    if (contactId) {
      getContact();
    } else {
      setIsLoading(false);
    }
  }, []);

  return { isLoading, contact };
};

export default useContact;
