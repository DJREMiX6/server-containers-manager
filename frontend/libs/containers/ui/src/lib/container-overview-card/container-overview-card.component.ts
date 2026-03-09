import { Component, computed, input, output } from '@angular/core';
import { CardModule } from 'primeng/card';
import { TagModule } from 'primeng/tag';
import { ButtonModule } from 'primeng/button';
import { SkeletonModule } from 'primeng/skeleton';
import { ContainerOverviewInfo } from '../models';

@Component({
  selector: 'lib-container-overview-card',
  imports: [CardModule, TagModule, ButtonModule, SkeletonModule],
  templateUrl: './container-overview-card.component.html',
  styleUrl: './container-overview-card.component.css',
})
export class ContainerOverviewCardComponent {
  public readonly containerInfo = input.required<ContainerOverviewInfo>();

  public readonly startStopBtnClick = output<ContainerOverviewInfo>();
  public readonly playPauseBtnClick = output<ContainerOverviewInfo>();
  public readonly restartBtnClick = output<ContainerOverviewInfo>();
  public readonly killBtnClick = output<ContainerOverviewInfo>();

  protected readonly showStartBtn = computed(() => {
    switch(this.containerInfo().state) {
      case "created": return true;
      case "dead": return true;
      case "exited": return true;
      default: return false;
    }
  });

  protected readonly showStopBtn = computed(() => {
    switch(this.containerInfo().state) {
      case "paused": return true;
      case "running": return true;
      case "restarting": return true;
      default: return false;
    }
  });

  protected readonly showPauseBtn = computed(() => {
    switch(this.containerInfo().state) {
      case "running": return true;
      default: return false;
    }
  });

  protected readonly showResumePlayBtn = computed(() => {
    switch(this.containerInfo().state) {
      case "paused": return true;
      default: return false;
    }
  });

  protected readonly playPauseBtnIcon = computed(() => {
    if(this.showResumePlayBtn()) return "pi pi-play";
    else return "pi pi-pause";
  })

  protected readonly showRestartBtn = computed(() => {
    switch(this.containerInfo().state) {
      case "paused": return true;
      case "running": return true;
      default: return false;
    }
  });

  protected readonly showKillBtn = computed(() => {
    switch(this.containerInfo().state) {
      case "paused": return true;
      case "restarting": return true;
      case "running": return true;
      default: return false;
    }
  });
}
