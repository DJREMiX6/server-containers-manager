import { Provider } from "@angular/core";
import { ContainersService } from "@scm/containers/data";
import { ContainersOverviewStore } from "../stores";

export function provideContainersOverviewStore(): Provider[] {
    return [
        ContainersService,
        ContainersOverviewStore
    ];
}