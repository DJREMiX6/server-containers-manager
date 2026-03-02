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
import { form, FormField, required, submit } from '@angular/forms/signals';
import { LoginFormModel } from '../models/login-form-model';

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
  private loginModel = signal<LoginFormModel>({
    username: '',
    password: '',
  });

  protected loginForm = form(this.loginModel, (schema) => {
    required(schema.username, { message: 'Username is required' });
    required(schema.password, { message: 'Password is required' });
  });

  protected loginBtnClicked_evt() {
    submit(this.loginForm, async (form) => {
      console.log('test');
    });
  }
}
