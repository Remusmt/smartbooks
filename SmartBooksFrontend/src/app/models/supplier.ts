import { Address } from './address';

export interface Supplier {
  id: number;
  name: string;
  currencyId: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  email: string;
  idNumber: string;
  defaultAddressId?: number;
  categoryId?: number;
  paymentTermId?: number;
  creditLimit: number;
  creditLimitPeriod: number;
  ledgerAccountId?: number;
  balance: number;
  currencyBalance: number;
  accountNumber: string;
  vatNumber: string;
  pin: string;
  bankId: number;
  bankBranchId: number;
  bankAccountNumber: string;
  isTaxAgency: boolean;
  organisationAddresses: Array<Address>;
}

export interface SuppliersList {
  organisations: Array<Supplier>;
  totalCount: number;
}
