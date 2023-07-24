import { useState } from "react";

import { AddressesService } from "src/services";

const useUpdateAddress = () => {
  const [isUpdating, setIsUpdating] = useState(false);

  const updateAddress = async (addressId, address) => {
    setIsUpdating(true);

    let responseCode;

    try {
      const response = await AddressesService.update(addressId, address);

      responseCode = response.status;
    } catch (error) {
      responseCode = error.response.status;
    }

    setIsUpdating(false);

    return { responseCode };
  };

  return { isUpdating, updateAddress };
};

export default useUpdateAddress;
