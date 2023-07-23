import { useState } from "react";

import { AddressesService } from "src/services";

const useCreateAddress = () => {
  const [isCreating, setIsCreating] = useState(false);

  const createAddress = async (address) => {
    setIsCreating(true);

    let responseCode;

    try {
      const response = await AddressesService.create(address);

      responseCode = response.status;
    } catch (error) {
      responseCode = error.response.status;
    }

    setIsCreating(false);

    return { responseCode };
  };

  return { isCreating, createAddress };
};

export default useCreateAddress;
