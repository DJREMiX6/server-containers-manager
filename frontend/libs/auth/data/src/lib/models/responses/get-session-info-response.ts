import { NamespaceInfo } from "../namespace-info";

export type GetSessionInfoResponse = {
    userId: string;
    username: string;
    roles: string[];
    namespaces: NamespaceInfo[];
}