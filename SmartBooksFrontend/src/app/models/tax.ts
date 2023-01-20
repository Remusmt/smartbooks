export interface Tax {
  id: number;
  code: string;
  description: string;
  taxRegistrationNumber: string;
  taxAgencyId: number;
  reportingMethod: number;
  purchasesAccountId?: number;
  salesAccountId?: number;
  taxRates: Array<TaxRate>;
}

export interface TaxRate {
  id: number;
  taxId: number;
  description: string;
  salesRate: number;
  purchaseRate: number;
  purchaseTaxIsReclaimable: boolean;
}
