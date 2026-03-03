import {
  Component,
  ChangeDetectionStrategy,
  signal,
  inject,
} from '@angular/core';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { InputTextModule } from 'primeng/inputtext';
import { MessageModule } from 'primeng/message';
import { ButtonModule } from 'primeng/button';
import { FloatLabelModule } from 'primeng/floatlabel';
import { MessageService } from 'primeng/api';
import { form, FormField, required, submit } from '@angular/forms/signals';
import { LoginFormModel } from '../models/login-form-model';
import { AuthStore, LoginRequestModel } from '@scm/auth/state';
import { Router } from '@angular/router';

@Component({
  selector: 'lib-login-feature.component',
  imports: [
    IconFieldModule,
    InputIconModule,
    InputTextModule,
    ButtonModule,
    MessageModule,
    FloatLabelModule,
    FormField,
  ],
  templateUrl: './login-feature.component.html',
  styleUrl: './login-feature.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginFeatureComponent {
  private readonly authStore = inject(AuthStore);
  private readonly toastService = inject(MessageService);
  private readonly router = inject(Router);

  protected isLoginBuisy = signal<boolean>(false);

  private loginModel = signal<LoginFormModel>({
    username: '',
    password: '',
  });

  protected loginForm = form(this.loginModel, (schema) => {
    required(schema.username, { message: 'Username is required' });
    required(schema.password, { message: 'Password is required' });
  });

  protected loginBtnClicked_evt() {
    if (this.isLoginBuisy()) return;

    this.isLoginBuisy.set(true);

    submit(this.loginForm, async (form) => {
      try {
        const loginRequest: LoginRequestModel = {
          username: form().value().username,
          password: form().value().password,
        };

        await this.authStore.login(loginRequest);
        this.toastService.add({
          summary: 'Login successful',
          severity: 'success',
        });

        this.router.navigate(['']);
      } catch (error) {
        console.error(error);

        this.toastService.add({
          summary: 'Invalid login credentials',
          severity: 'error',
        });
      }
    }).finally(() => this.isLoginBuisy.set(false));
  }
}
