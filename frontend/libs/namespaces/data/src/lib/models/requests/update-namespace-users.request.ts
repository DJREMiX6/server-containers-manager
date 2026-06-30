export type UpdateNamespaceUsersRequest = {
  namespaceId: string;
  data: UpdateNamespaceUsersRequestData
}

export type UpdateNamespaceUsersRequestData = {
  associatedUserIds: string[];
}