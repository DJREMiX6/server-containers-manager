import { Component, computed, input, output, Signal } from '@angular/core';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { SkeletonModule } from 'primeng/skeleton';
import { ChartModule } from 'primeng/chart';
import { ContainerOverviewInfo, ContainerResourcesData } from '../models';
import { ChartOptions, ChartData, Point } from 'chart.js';

@Component({
  selector: 'lib-container-overview-card',
  imports: [CardModule, TagModule, ButtonModule, SkeletonModule, ChartModule],
  templateUrl: './container-overview-card.component.html',
  styleUrl: './container-overview-card.component.css',
})
export class ContainerOverviewCardComponent {
  public readonly containerInfo = input.required<ContainerOverviewInfo>();
  public readonly containerResourcesData =
    input.required<ContainerResourcesData>();

  public readonly startStopBtnClick = output<ContainerOverviewInfo>();
  public readonly playPauseBtnClick = output<ContainerOverviewInfo>();
  public readonly restartBtnClick = output<ContainerOverviewInfo>();
  public readonly killBtnClick = output<ContainerOverviewInfo>();

  private readonly documentStyle = getComputedStyle(
    globalThis.document.documentElement,
  );

  protected readonly showStartBtn = computed(() => {
    switch (this.containerInfo().state) {
      case 'Created':
        return true;
      case 'Dead':
        return true;
      case 'Exited':
        return true;
      default:
        return false;
    }
  });

  protected readonly showStopBtn = computed(() => {
    switch (this.containerInfo().state) {
      case 'Paused':
        return true;
      case 'Running':
        return true;
      case 'Restarting':
        return true;
      default:
        return false;
    }
  });

  protected readonly showPauseBtn = computed(() => {
    switch (this.containerInfo().state) {
      case 'Running':
        return true;
      default:
        return false;
    }
  });

  protected readonly showResumePlayBtn = computed(() => {
    switch (this.containerInfo().state) {
      case 'Paused':
        return true;
      default:
        return false;
    }
  });

  protected readonly playPauseBtnIcon = computed(() => {
    if (this.showResumePlayBtn()) return 'pi pi-play';
    else return 'pi pi-pause';
  });

  protected readonly showRestartBtn = computed(() => {
    switch (this.containerInfo().state) {
      case 'Paused':
        return true;
      case 'Running':
        return true;
      default:
        return false;
    }
  });

  protected readonly showKillBtn = computed(() => {
    switch (this.containerInfo().state) {
      case 'Paused':
        return true;
      case 'Restarting':
        return true;
      case 'Running':
        return true;
      default:
        return false;
    }
  });

  private readonly chartsLabels: Signal<number[]> = computed(() => {
    {
      const containerResourcesData = this.containerResourcesData();
      const undistinctLabels = [
        containerResourcesData.cpuUsagePercentage,
        containerResourcesData.diskUsagePercentage,
        containerResourcesData.memoryUsagePercentage,
        containerResourcesData.networkUsagePercentage,
      ]
        .flat()
        .map((p) => p.x)
        .filter((x) => x !== null)
        .sort((a, b) => a - b);

      return [...new Set<number>(undistinctLabels)];
    }
  });

  protected readonly cpuChart: Signal<ChartData<'line', Point[]>> = computed(
    () => {
      const cpuUsagePercentage =
        this.containerResourcesData().cpuUsagePercentage;
      const labels = this.chartsLabels();
      return {
        labels,
        datasets: [
          {
            label: 'CPU',
            data: cpuUsagePercentage,
            fill: false,
            borderColor: this.documentStyle.getPropertyValue('--p-orange-500'),
            borderWidth: 2,
            pointRadius: 0,
            pointHoverRadius: 0,
            tension: 0.1,
          },
        ],
      };
    },
  );

  protected readonly memoryChart: Signal<ChartData<'line', Point[]>> = computed(
    () => {
      const memoryUsagePercentage =
        this.containerResourcesData().memoryUsagePercentage;
      const labels = this.chartsLabels();
      return {
        labels,
        datasets: [
          {
            label: 'Memory',
            data: memoryUsagePercentage,
            fill: false,
            borderColor: this.documentStyle.getPropertyValue('--p-purple-500'),
            borderWidth: 2,
            pointRadius: 0,
            pointHoverRadius: 0,
            tension: 0.1,
          },
        ],
      };
    },
  );

  protected readonly diskChart: Signal<ChartData<'line', Point[]>> = computed(
    () => {
      const diskUsagePercentage =
        this.containerResourcesData().diskUsagePercentage;
      const labels = this.chartsLabels();
      return {
        labels,
        datasets: [
          {
            label: 'Disk',
            data: diskUsagePercentage,
            fill: false,
            borderColor: this.documentStyle.getPropertyValue('--p-cyan-500'),
            borderWidth: 2,
            pointRadius: 0,
            pointHoverRadius: 0,
            tension: 0.1,
          },
        ],
      };
    },
  );

  protected readonly networkChart: Signal<ChartData<'line', Point[]>> =
    computed(() => {
      const networkUsagePercentage =
        this.containerResourcesData().networkUsagePercentage;
      const labels = this.chartsLabels();
      return {
        labels,
        datasets: [
          {
            label: 'Network',
            data: networkUsagePercentage,
            fill: false,
            borderColor: this.documentStyle.getPropertyValue('--p-green-500'),
            borderWidth: 2,
            pointRadius: 0,
            pointHoverRadius: 0,
            tension: 0.1,
          },
        ],
      };
    });

  protected readonly chartOptions: ChartOptions<'line'> = {
    responsive: true,
    maintainAspectRatio: false,
    animation: false,
    normalized: true,
    plugins: {
      legend: {
        display: false,
      },
      tooltip: {
        enabled: false,
      },
    },
    layout: {
      padding: 0,
    },
    scales: {
      x: {
        display: true,
      },
      y: {
        display: true,
      },
    },
  };
}
