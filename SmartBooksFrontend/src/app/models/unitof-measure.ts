export interface UnitofMeasure {
  id: number;
  abbreviation: string;
  description: string;
  type: number;
  uomConversions: Array<UomConversion>;
}

export interface UomConversion {
  id: number;
  unitofMeasureFromId: number;
  unitofMeasureToId: number;
  description: string;
  factor: number;
}
