import { Component, computed, effect, inject, OnInit } from '@angular/core';
import { MessageService } from 'primeng/api';
import {
  ContainerOverviewCardComponent,
  ContainerOverviewInfo,
  ContainerResourcesData,
} from '@scm/containers/ui';
import {
  provideContainersOverviewStore,
  ContainersOverviewStore,
} from '@scm/containers/store';

type ContainerOverview = {
  info: ContainerOverviewInfo;
  resourcesUsage: ContainerResourcesData;
};

@Component({
  selector: 'lib-containers-overview',
  imports: [ContainerOverviewCardComponent],
  providers: [...provideContainersOverviewStore()],
  templateUrl: './containers-overview.component.html',
  styleUrl: './containers-overview.component.css',
})
export class ContainersOverviewComponent implements OnInit {
  private readonly toastService = inject(MessageService);
  protected readonly containersOverviewStore = inject(ContainersOverviewStore);

  protected readonly containersResources: ContainerResourcesData[] = [
    {
      cpuUsagePercentage: [
        { x: 1, y: 9 },
        { x: 5, y: 11 },
        { x: 8, y: 13 },
        { x: 11, y: 19 },
        { x: 13, y: 22 },
        { x: 18, y: 20 },
        { x: 24, y: 28 },
        { x: 27, y: 32 },
      ],
      diskUsagePercentage: [
        { x: 2, y: 9 },
        { x: 5, y: 14 },
        { x: 7, y: 11 },
        { x: 11, y: 20 },
        { x: 14, y: 24 },
        { x: 18, y: 19 },
        { x: 21, y: 27 },
        { x: 25, y: 31 },
      ],
      memoryUsagePercentage: [
        { x: 1, y: 6 },
        { x: 3, y: 8 },
        { x: 6, y: 15 },
        { x: 10, y: 13 },
        { x: 13, y: 18 },
        { x: 17, y: 22 },
        { x: 22, y: 25 },
        { x: 26, y: 21 },
      ],
      networkUsagePercentage: [
        { x: 4, y: 12 },
        { x: 8, y: 10 },
        { x: 9, y: 16 },
        { x: 12, y: 19 },
        { x: 16, y: 17 },
        { x: 19, y: 23 },
        { x: 23, y: 26 },
        { x: 28, y: 29 },
      ],
    },
    {
      cpuUsagePercentage: [
        { x: 3, y: 5 },
        { x: 4, y: 9 },
        { x: 7, y: 14 },
        { x: 9, y: 12 },
        { x: 12, y: 20 },
        { x: 15, y: 18 },
        { x: 20, y: 24 },
        { x: 24, y: 30 },
      ],
      diskUsagePercentage: [
        { x: 5, y: 7 },
        { x: 6, y: 11 },
        { x: 10, y: 13 },
        { x: 13, y: 21 },
        { x: 17, y: 20 },
        { x: 18, y: 26 },
        { x: 22, y: 28 },
        { x: 27, y: 33 },
      ],
      memoryUsagePercentage: [
        { x: 2, y: 4 },
        { x: 4, y: 10 },
        { x: 7, y: 9 },
        { x: 8, y: 15 },
        { x: 11, y: 19 },
        { x: 14, y: 17 },
        { x: 19, y: 25 },
        { x: 23, y: 27 },
      ],
      networkUsagePercentage: [
        { x: 1, y: 8 },
        { x: 2, y: 12 },
        { x: 5, y: 14 },
        { x: 9, y: 16 },
        { x: 12, y: 22 },
        { x: 16, y: 24 },
        { x: 21, y: 23 },
        { x: 24, y: 29 },
      ],
    },
    {
      cpuUsagePercentage: [
        { x: 3, y: 6 },
        { x: 6, y: 9 },
        { x: 8, y: 15 },
        { x: 12, y: 14 },
        { x: 15, y: 22 },
        { x: 19, y: 25 },
        { x: 24, y: 21 },
        { x: 29, y: 30 },
      ],
      diskUsagePercentage: [
        { x: 2, y: 11 },
        { x: 5, y: 13 },
        { x: 9, y: 10 },
        { x: 11, y: 18 },
        { x: 14, y: 23 },
        { x: 18, y: 20 },
        { x: 20, y: 27 },
        { x: 23, y: 31 },
      ],
      memoryUsagePercentage: [
        { x: 4, y: 7 },
        { x: 7, y: 12 },
        { x: 10, y: 16 },
        { x: 13, y: 13 },
        { x: 17, y: 19 },
        { x: 22, y: 24 },
        { x: 26, y: 28 },
        { x: 31, y: 26 },
      ],
      networkUsagePercentage: [
        { x: 1, y: 5 },
        { x: 4, y: 8 },
        { x: 6, y: 14 },
        { x: 9, y: 17 },
        { x: 12, y: 15 },
        { x: 16, y: 22 },
        { x: 21, y: 27 },
        { x: 25, y: 29 },
      ],
    },
    {
      cpuUsagePercentage: [
        { x: 5, y: 10 },
        { x: 8, y: 9 },
        { x: 11, y: 15 },
        { x: 15, y: 18 },
        { x: 18, y: 24 },
        { x: 23, y: 22 },
        { x: 27, y: 30 },
        { x: 30, y: 34 },
      ],
      diskUsagePercentage: [
        { x: 2, y: 3 },
        { x: 3, y: 7 },
        { x: 7, y: 11 },
        { x: 10, y: 16 },
        { x: 14, y: 18 },
        { x: 17, y: 23 },
        { x: 22, y: 20 },
        { x: 28, y: 27 },
      ],
      memoryUsagePercentage: [
        { x: 6, y: 8 },
        { x: 9, y: 12 },
        { x: 12, y: 10 },
        { x: 16, y: 17 },
        { x: 19, y: 21 },
        { x: 21, y: 26 },
        { x: 25, y: 24 },
        { x: 32, y: 33 },
      ],
      networkUsagePercentage: [
        { x: 1, y: 9 },
        { x: 5, y: 11 },
        { x: 8, y: 13 },
        { x: 11, y: 19 },
        { x: 13, y: 22 },
        { x: 18, y: 20 },
        { x: 24, y: 28 },
        { x: 27, y: 32 },
      ],
    },
  ];

  protected readonly containersOverview = computed((): ContainerOverview[] => {
    if (this.containersOverviewStore.loadingStatus() !== 'loaded') return [];

    const containers = this.containersOverviewStore.containers();

    return containers.map(
      (container, i): ContainerOverview => ({
        info: {
          id: container.id,
          name: container.name,
          state: container.state,
          namespaces: container.namespaces.map((n) => n.name),
        },
        resourcesUsage: this.containersResources[i],
      }),
    );
  });

  private readonly onContainersOverviewStoreError = effect(() => {
    const error = this.containersOverviewStore.error();
    if (error === null) return;

    this.toastService.add({
      summary: 'Error',
      detail: error.summary,
      severity: 'error',
    });
  });

  ngOnInit(): void {
    this.containersOverviewStore.ensureLoaded();
  }

  protected async startBtnClick_evt(containerInfo: ContainerOverviewInfo) {
    await this.containersOverviewStore.startContainer(containerInfo.id);
  }

  protected async stopBtnClick_evt(containerInfo: ContainerOverviewInfo) {
    await this.containersOverviewStore.stopContainer(containerInfo.id);
  }

  protected resumeBtnClick_evt(containerInfo: ContainerOverviewInfo) {
    console.log('ResumePauseBtn', containerInfo.id);
  }

  protected pauseBtnClick_evt(containerInfo: ContainerOverviewInfo) {
    console.log('PausePauseBtn', containerInfo.id);
  }

  protected restartBtnClick_evt(containerInfo: ContainerOverviewInfo) {
    console.log('RestartBtn', containerInfo.id);
  }

  protected killBtnClick_evt(containerInfo: ContainerOverviewInfo) {
    console.log('KillBtn', containerInfo.id);
  }
}
