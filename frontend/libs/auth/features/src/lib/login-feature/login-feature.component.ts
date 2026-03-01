import { Component, ChangeDetectionStrategy } from '@angular/core';
import { IconFieldModule } from "primeng/iconfield";
import { InputIconModule } from "primeng/inputicon";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from "primeng/button";

@Component({
  selector: 'lib-login-feature.component',
  imports: [IconFieldModule, InputIconModule, InputTextModule, ButtonModule],
  templateUrl: './login-feature.component.html',
  styleUrl: './login-feature.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginFeatureComponent {}
