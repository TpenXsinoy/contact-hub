import { useContext, useState } from "react";
import { UserContext } from "src/contexts";
import { UsersService } from "src/services";

const useUpdateUser = () => {
  const { user } = useContext(UserContext);
  const [isUpdating, setIsUpdating] = useState(false);
  const [isVerifyingPassword, setIsVerifyingPassword] = useState(false);

  const updateUser = async (userId, userTobeUpdated, oldPassword = null) => {
    setIsUpdating(true);

    let responseCode;
    let responseData;
    const errors = {};
    try {
      setIsVerifyingPassword(true);

      // Call the login API to verify if the password is correct
      await UsersService.login({
        username: user.username,
        password: oldPassword || userTobeUpdated.password,
      });

      setIsVerifyingPassword(false);

      // If the password is correct, call the API to update the user
      const response = await UsersService.update(userId, userTobeUpdated);
      responseCode = response.status;
    } catch (error) {
      responseData = error.response.data;
      responseCode = error.response.status;

      if (responseData.includes("Wrong password")) {
        errors.password =
          "Incorrect Password: Input the correct password to update your account.";
      }

      if (responseData.includes("Username")) {
        errors.username = "Username is already taken.";
      }

      if (responseData.includes("Email")) {
        errors.email = "Email is already taken.";
      }
    }

    setIsUpdating(false);

    return { responseCode, errors };
  };

  return { isUpdating, isVerifyingPassword, updateUser };
};

export default useUpdateUser;
