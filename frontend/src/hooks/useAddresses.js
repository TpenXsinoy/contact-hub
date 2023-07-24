import { useAlert } from "react-alert";

import { AddressesService } from "src/services";

const useAddresses = (setAddresses) => {
  const alert = useAlert();

  const deleteAddress = async (addressId) => {
    const { status: deleteAddressStatus } = await AddressesService.delete(
      addressId
    );

    if (deleteAddressStatus === 200) {
      alert.success("Address deleted.");

      setAddresses((prevAddresses) =>
        prevAddresses.filter((prevAddress) => prevAddress.id !== addressId)
      );
    } else {
      alert.error("Oops, something went wrong.");
    }
  };

  return { deleteAddress };
};

export default useAddresses;
