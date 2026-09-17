import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
} from '@angular/core';
import { Button } from 'primeng/button';
import { InputText } from 'primeng/inputtext';
import { InputGroupAddon } from 'primeng/inputgroupaddon';
import { InputGroup } from 'primeng/inputgroup';

@Component({
  selector: 'lib-copy-text-field',
  imports: [Button, InputText, InputGroupAddon, InputGroup],
  templateUrl: './copy-text-field.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CopyTextField {
  public readonly password = input.required<string>();

  public readonly passwordCopied = output<void>();

  protected async onCopyPasswordBtnClick(): Promise<void> {
    const password = this.password();

    if (globalThis.isSecureContext) await this.secureContextCopy(password);
    else this.unsecureContextCopy(password);

    this.passwordCopied.emit();
  }

  private async secureContextCopy(text: string): Promise<void> {
    await globalThis.navigator.clipboard.writeText(text);
  }

  private unsecureContextCopy(text: string): void {
    const textarea = document.createElement('textarea');
    textarea.value = text;
    textarea.style.position = 'fixed';
    textarea.style.opacity = '0';
    document.body.appendChild(textarea);
    textarea.focus();
    textarea.select();

    try {
      document.execCommand('copy');
    } finally {
      document.body.removeChild(textarea);
    }
  }
}
