import { User } from "../models/user";

export type AuthState = {
    isAuthenticated: boolean,
    user: User | null,
    error: Error | null
};

export const initialState: AuthState = {
    isAuthenticated: false,
    user: null,
    error: null
}