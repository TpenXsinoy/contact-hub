import { isLocal } from "../utils/destinations";

// Url of the API in Azure
let apiUrl = "https://contacthubapi.azurewebsites.net";

if (isLocal) {
  apiUrl = "http://localhost:80";
}

const config = {
  API_URL: apiUrl,
};

export default config;
