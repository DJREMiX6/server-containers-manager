import { Point } from "chart.js"

export type ContainerResourcesData = {
    cpuUsagePercentage: Point[];
    memoryUsagePercentage: Point[];
    diskUsagePercentage: Point[];
    networkUsagePercentage: Point[];
}