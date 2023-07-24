import { isLocal } from "../utils/destinations";

// Url of the API in Azure
let apiUrl = "https://contacthubapi.azurewebsites.net";

if (isLocal) {
  apiUrl = "https://localhost:7130";
}

const config = {
  API_URL: apiUrl,
};

export default config;
