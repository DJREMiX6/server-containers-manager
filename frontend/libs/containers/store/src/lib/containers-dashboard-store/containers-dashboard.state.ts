import { ContainerSummary } from "../models"

export type ContainersDashboardState = {
    _containersToLoad: number;
    _loadedAt: Date | null;
    loadingStatus: "notLoaded" | "loading" | "loaded";
    containers: ContainerSummary[];
    error: Error | null;
};

export const initialState: ContainersDashboardState = {
    _containersToLoad: 4,
    _loadedAt: null,
    loadingStatus: "notLoaded",
    containers: [],
    error: null
};

export function withContainersDashboardState() {
}