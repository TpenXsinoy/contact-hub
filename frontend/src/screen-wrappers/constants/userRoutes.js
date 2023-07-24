const BASE_ROUTE = "/user";

const USER_ROUTES = {
  CONTACTS: `${BASE_ROUTE}/contacts`,
  CREATE_CONTACT: `${BASE_ROUTE}/contacts/create`,
  UPDATE_CONTACT: `${BASE_ROUTE}/contacts/:contactId/update`,
  VIEW_CONTACT: `${BASE_ROUTE}/contacts/:contactId`,
  CREATE_ADDRESS: `${BASE_ROUTE}/contacts/:contactId/addresses/create`,
  UPDATE_ADDRESS: `${BASE_ROUTE}/contacts/:contactId/addresses/:addressId/update`,
  SETTINGS: `${BASE_ROUTE}/settings/:activeTab`,
};

export default USER_ROUTES;
