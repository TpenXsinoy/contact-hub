import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/api/confirmation-codes`;

const ConfirmationCodesService = {
  send: (email) =>
    axios.post(`${BASE_URL}/send`, null, {
      params: {
        email: email,
      },
    }),
  verify: (request) => axios.post(`${BASE_URL}/verify`, request),
};

export default ConfirmationCodesService;
