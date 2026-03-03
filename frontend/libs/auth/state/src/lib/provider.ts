import { inject, makeEnvironmentProviders, provideAppInitializer } from "@angular/core"
import { AuthService } from "@scm/auth/data";
import { AuthStore } from "./auth-state/auth.store";


export function provideAuthentication() {
    return makeEnvironmentProviders([
        AuthService,
        AuthStore,
        provideAuthInitializer()
    ])
}

function provideAuthInitializer() {
    return provideAppInitializer(() => {
        const authStore = inject(AuthStore);
        return authStore.checkAuth();
    });
}