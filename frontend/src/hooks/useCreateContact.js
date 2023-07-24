import { useState } from "react";

import { ContactsService } from "src/services";

const useCreateContact = () => {
  const [isCreating, setIsCreating] = useState(false);

  const createContact = async (contact) => {
    setIsCreating(true);

    let responseCode;

    try {
      const response = await ContactsService.create(contact);

      responseCode = response.status;
    } catch (error) {
      responseCode = error.response.status;
    }

    setIsCreating(false);

    return { responseCode };
  };

  return { isCreating, createContact };
};

export default useCreateContact;
