export const environment = {
  production: true,
  roles: {
    admin: "Admin",
    organizationAdmin: "OrganizationAdmin",
  },
  keycloak: {
    realm: "fahrzeugverwaltung",
    clientId: "fahrzeugverwaltung-dev",
    url: "https://keycloak.feuerwehr-winterberg.de",
  }
};
