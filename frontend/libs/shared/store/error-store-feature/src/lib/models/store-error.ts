import { ApiError } from "./api-error";
import { NetworkError } from "./network-error";
import { UnknownError } from "./unknown-error";
import { ValidationError } from "./validation-error";

export type StoreError =
  | ApiError
  | NetworkError
  | ValidationError
  | UnknownError;