export interface Warehouse {
  id: number;
  code: string;
  description: string;
  defaultReceivingBin: number;
  defaultDespatchBin: number;
  bins: Array<Bin>;
}

export interface Bin {
  id: number;
  warehouseId: number;
  code: string;
  description: string;
}
