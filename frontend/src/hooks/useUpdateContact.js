import { useState } from "react";

import { ContactsService } from "src/services";

const useUpdateContact = () => {
  const [isUpdating, setIsUpdating] = useState(false);

  const updateContact = async (contactId, contact) => {
    setIsUpdating(true);

    let responseCode;

    try {
      const response = await ContactsService.update(contactId, contact);

      responseCode = response.status;
    } catch (error) {
      responseCode = error.response.status;
    }

    setIsUpdating(false);

    return { responseCode };
  };

  return { isUpdating, updateContact };
};

export default useUpdateContact;
