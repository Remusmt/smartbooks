export interface LedgerAccount {
  id: number;
  accountType: number;
  accountNumber: string;
  accountName: string;
  description: string;
  currencyId: number;
  parentAccountId?: number;
  taxId?: number;
  cashBookAccountType: number;
  bankId?: number;
  bankBranchId?: number;
  bankAccountNo: string;
  hasOverDraft: boolean;
  overDraftLimit: number;
  balance: number;
  currencyBalance: number;
  height: number;
  totalBalance: number;
  totalCurrencyBalance: number;
  childAccounts: Array<LedgerAccount>;
}

export interface TreeLedgerAccountNode {
  expandable: boolean;
  level: number;
  id: number;
  accountType: number;
  accountNumber: string;
  accountName: string;
  description: string;
  currencyId: number;
  parentAccountId?: number;
  taxId?: number;
  cashBookAccountType: number;
  bankId?: number;
  bankBranchId?: number;
  bankAccountNo: string;
  hasOverDraft: boolean;
  overDraftLimit: number;
  balance: number;
  currencyBalance: number;
  height: number;
  totalBalance: number;
  totalCurrencyBalance: number;
}
