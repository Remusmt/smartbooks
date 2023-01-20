
export interface PaymentTerms {
  id: number;
  description: string;
  dateDriven: boolean;
  netDueIn: number;
  discountPercentage: number;
  discountIfPaidWithin: number;
  netDueBefore: number;
  dueNextMonthIfIssued: number;
  discountIfPaidBefore: number;
}
