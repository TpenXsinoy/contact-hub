import { isLocal } from "../utils/destinations";

let apiUrl = null;

if (isLocal) {
  apiUrl = "https://localhost:7130";
}

const config = {
  API_URL: apiUrl,
};

export default config;
