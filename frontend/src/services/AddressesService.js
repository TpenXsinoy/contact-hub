import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/api/addresses`;

const AddressesService = {
  create: (address) => axios.post(`${BASE_URL}`, address),
  retrieve: (id) => axios.get(`${BASE_URL}/${id}`),
  update: (id, address) => axios.put(`${BASE_URL}/${id}`, address),
  delete: (id) => axios.delete(`${BASE_URL}/${id}`),
};

export default AddressesService;
