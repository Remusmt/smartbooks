import { NumberValueAccessor } from '@angular/forms';
import { Address } from './address';

export interface Customer {
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
  organisationAddresses: Array<Address>;
}

export interface CustomersList {
  organisations: Array<Customer>;
  totalCount: number;
}

