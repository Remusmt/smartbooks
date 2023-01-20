export interface UserModel {
  email: string;
  fullName: string;
  phoneNumber: string;
  userRights: Array<UserRightGroup>;
}

export interface UserRightGroup {
  description: string;
  claimType: string;
  granted: boolean;
  rights?: Array<UserRightGroup>;
}

export interface TreeUserRightNode {
  expandable: boolean;
  description: string;
  claimType: string;
  granted: boolean;
  level: number;
}

export interface Claim {
  type: string;
  value: string;
}
