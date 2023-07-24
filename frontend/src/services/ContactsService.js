import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/api/contacts`;

const ContactsService = {
  create: (contact) => axios.post(`${BASE_URL}`, contact),
  list: () => axios.get(`${BASE_URL}`),
  retrieve: (id) => axios.get(`${BASE_URL}/${id}`),
  update: (id, contact) => axios.put(`${BASE_URL}/${id}`, contact),
  delete: (id) => axios.delete(`${BASE_URL}/${id}`),
};

export default ContactsService;
