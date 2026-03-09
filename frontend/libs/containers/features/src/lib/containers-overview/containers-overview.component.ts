import { Component } from '@angular/core';
import {
  ContainerOverviewCardComponent,
  ContainerOverviewInfo,
} from '@scm/containers/ui';

@Component({
  selector: 'lib-containers-overview',
  imports: [ContainerOverviewCardComponent],
  templateUrl: './containers-overview.component.html',
  styleUrl: './containers-overview.component.css',
})
export class ContainersOverviewComponent {
  protected readonly containers: ContainerOverviewInfo[] = [
    {
      id: '1',
      name: 'My App A',
      namespaces: ['App A'],
      state: 'running',
    },
    {
      id: '2',
      name: 'MC Eternal 2',
      namespaces: ['Games', 'Minecraft'],
      state: 'paused',
    },
    {
      id: '3',
      name: 'V Rising',
      namespaces: ['Games', 'V Rising'],
      state: 'removing',
    },
    {
      id: '4',
      name: 'My App B',
      namespaces: ['App B'],
      state: 'dead',
    },
  ];

  protected startStopBtnClick_evt(containerId: string) {
    console.log('StartStopBtn', containerId);
  }

  protected playPauseBtnClick_evt(containerId: string) {
    console.log('PlayPauseBtn', containerId);
  }

  protected restartBtnClick_evt(containerId: string) {
    console.log('RestartBtn', containerId);
  }

  protected killBtnClick_evt(containerId: string) {
    console.log('KillBtn', containerId);
  }
}
