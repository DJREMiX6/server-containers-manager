export type UpdateNamespaceAssociatedContainersRequest = {
  namespaceId: string;
  data: UpdateNamespaceAssociatedContainersRequestData
}

export type UpdateNamespaceAssociatedContainersRequestData = {
  AssociatedContainersIds: string[];
}