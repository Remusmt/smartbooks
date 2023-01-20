import { NumberValueAccessor } from '@angular/forms';

export interface Bank {
  id: number;
  countryId: number;
  bankCode: string;
  bankName: string;
  bankBranches: Array<BankBranch>;
}

export interface BankBranch {
  id: number;
  bankId: number;
  branchCode: string;
  branchName: string;
  swiftCode: string;
}

