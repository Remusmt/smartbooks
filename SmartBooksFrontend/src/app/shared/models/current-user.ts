export interface CurrentUser {
  id: number;
  email: string;
  fullName: string;
  phoneNumber: string;
  tokenString: string;
  companyId: number;
  companyName: string;
  succeeded: boolean;
  canManageUsers: boolean;
  companyDefault: CompanyDefault;
  menus: Array<MenuNode>;
}

export interface MenuNode {
  description: string;
  linkUrl: string;
  icon: string;
  imageIcon: boolean;
  links?: MenuNode[];
}

export interface FlatTreeMenuNode {
  expandable: boolean;
  description: string;
  linkUrl: string;
  icon: string;
  imageIcon: boolean;
  level: number;
}

export interface CompanyDefault {
  defaultCurrency: number;
  useAccountNumbers: boolean;
  allowPostingToParentAccount: boolean;
  enableInventoryTracking: boolean;
  defaultWarehouse: number;
  hasMultipleWarehouses: boolean;
}
