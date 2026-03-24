import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { SideNavMenuComponent } from '../../ui/side-nav-menu/side-nav-menu.component';
import { AuthStore } from '@scm/auth/store';

@Component({
  selector: 'app-shell',
  imports: [RouterModule, SideNavMenuComponent],
  templateUrl: './shell.component.html',
  styleUrl: './shell.component.css',
})
export class ShellComponent {
  protected readonly authStore = inject(AuthStore);
}
