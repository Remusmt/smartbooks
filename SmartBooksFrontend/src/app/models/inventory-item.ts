import { Category } from './category';
import { UnitofMeasure } from './unitof-measure';

export interface InventoryItem {
  id: number;
  code: string;
  description: string;
  type: number;
  unitofMeasureId: number;
  inventoryCategoryId?: number;
  taxId?: number;
  standardCost: number;
  standardPrice: number;
  assetAcount: number;
  cogsAccount: number;
  incomeAccount: number;
  onHand: number;
  onOrder: number;
  allocated: number;
  backOrdered: number;
  available: number;
  totalAverageCost: number;
  totalStandardCost: number;
  unitofMeasure: UnitofMeasure;
  inventoryCategory: Category;
}

export interface InventoryItemsList {
  inventoryItems: Array<InventoryItem>;
  totalCount: number;
}

