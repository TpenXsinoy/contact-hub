import { useState, useEffect } from "react";

import { AddressesService } from "src/services";

const useAddress = (addressId) => {
  const [isLoading, setIsLoading] = useState(true);
  const [address, setAddress] = useState(null);

  useEffect(() => {
    const getAddress = async () => {
      const { data: getAddressResponse } = await AddressesService.retrieve(
        addressId
      );

      if (getAddressResponse) {
        setAddress(getAddressResponse);
      }

      setIsLoading(false);
    };

    if (addressId) {
      getAddress();
    } else {
      setIsLoading(false);
    }
  }, []);

  return { isLoading, address };
};

export default useAddress;
