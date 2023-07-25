import { isLocal } from "../utils/destinations";

// Url of the API in Azure
let apiUrl = "https://contacthubapicontainer.azurewebsites.net";

if (isLocal) {
  // Change to "https://localhost:7130" when not in docker development
  apiUrl = "http://localhost:80";
}

const config = {
  API_URL: apiUrl,
};

export default config;
