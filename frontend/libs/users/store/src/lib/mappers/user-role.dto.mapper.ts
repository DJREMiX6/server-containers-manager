import { isUserRole, UserRole } from "../models";

export function userRoleMapper(value: string): UserRole {
  if (isUserRole(value)) return value;
  throw new Error(`Cannot cast '${value}' to UserRole`);
}