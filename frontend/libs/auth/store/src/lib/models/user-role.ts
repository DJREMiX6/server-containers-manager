const UserRoles = ['Admin', 'Member'] as const;
export type UserRole = (typeof UserRoles)[number];

export function toUserRole(value: string): UserRole {
  if (isUserRole(value)) return value;
  throw new Error(`Cannot cast '${value}' to UserRole`);
}

export function isUserRole(value: any): value is UserRole {
  if (UserRoles.includes(value)) return true;
  return false;
}
