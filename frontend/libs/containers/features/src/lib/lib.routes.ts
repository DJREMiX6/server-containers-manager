import { Route } from "@angular/router";
import { ContainersFeatures } from "./containers-features/containers-features";

export const routes: Route[] = [
    {
        path: "",
        children: [
            {
                path: "",
                component: ContainersFeatures
            }
        ]
    }
]