import { Injectable, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Environment } from "@scm/environments"; 
import { LoginRequest } from "../models/requests";

export const ApiBaseEndpoint = `${Environment.serverOrigin}/api/auth`;

@Injectable({
    providedIn: "root"
})
export class AuthService {
    private readonly httpClient = inject(HttpClient);

    public login(request: LoginRequest): Observable<void> {
        return this.httpClient.post<void>(`${ApiBaseEndpoint}/signin`, request);
    }
}