import axios from "axios";
import config from "./config";

const BASE_URL = `${config.API_URL}/api/users`;

const UsersService = {
  login: (user) => axios.post(`${BASE_URL}/login`, user),
  signup: (user) => axios.post(`${BASE_URL}/signup`, user),
  retrieveById: (id) => axios.get(`${BASE_URL}/${id}`),
  retrieveByUsername: (username) =>
    axios.get(`${BASE_URL}/${username}/details`),
  update: (id, user) => axios.put(`${BASE_URL}/${id}`, user),
};

export default UsersService;
