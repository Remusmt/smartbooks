export interface Address {
  id: number;
  location: string;
  poBox: string;
  postalCode: string;
  city: string;
  countryId?: number;
  organisationId: number;
}
