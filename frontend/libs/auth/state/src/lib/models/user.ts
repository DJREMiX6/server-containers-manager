import { Namespace } from "./namespace";
import { UserRole } from "./user-role";

export type User = {
    id: string;
    username: string;
    roles: UserRole[];
    namespaces: Namespace[];
}