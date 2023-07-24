import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/api/tokens`;

const TokensService = {
  acquire: (user) => axios.post(`${BASE_URL}/acquire`, user),
  renew: (refreshToken) => axios.post(`${BASE_URL}/renew`, refreshToken),
};

export default TokensService;
