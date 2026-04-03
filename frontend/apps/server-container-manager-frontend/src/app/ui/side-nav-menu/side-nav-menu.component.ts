import { ChangeDetectionStrategy, Component, computed, input, signal } from '@angular/core';
import { TooltipModule } from 'primeng/tooltip';
import { DividerModule } from "primeng/divider"
import { AvatarModule } from "primeng/avatar";
import { RouterModule } from '@angular/router';
import { NgClass } from "@angular/common"

type NavItem = {
  title: string;
  icon: string;
  routerLink: string,
  isActive: boolean
}

@Component({
  selector: 'app-side-nav-menu',
  imports: [TooltipModule, DividerModule, AvatarModule, RouterModule, NgClass],
  templateUrl: './side-nav-menu.component.html',
  styleUrl: './side-nav-menu.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SideNavMenuComponent {
  public readonly username = input.required<string>();

  protected readonly usernameInitials = computed(() => this.username().substring(0, 2));

  protected readonly navItems: NavItem[] = [
    { title: "Dashboard", icon: "pi pi-th-large", routerLink: "dashboard", isActive: false },
    { title: "Containers", icon: "pi pi-box", routerLink: "containers", isActive: false },
    { title: "Users", icon: "pi pi-users", routerLink: "users", isActive: false },
  ];

  protected routerLinkActiveChanges(itemTitle: string, isActive: boolean) {
    const navItem = this.navItems.find(i => i.title == itemTitle);
    if(!navItem) throw new Error("Invalid NavItem title");

    navItem.isActive = isActive;
  }

  protected ProfileBtnClick_evt() {
    console.error("Not implemented yet");
  }
}
