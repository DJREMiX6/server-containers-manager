const UserRoles = ['Admin', 'Member'] as const;
export type UserRole = (typeof UserRoles)[number];

export function isUserRole(value: any): value is UserRole {
  if (UserRoles.includes(value)) return true;
  return false;
}
