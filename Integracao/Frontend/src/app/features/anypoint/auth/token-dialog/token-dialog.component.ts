import { Component } from '@angular/core';
import { trigger, transition, style, animate } from '@angular/animations';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-token-dialog',
  template: `
    <div class="modal-backdrop" (click)="close()" [@backdropFade]></div>
    <div class="modal-container" [@modalAnimation] role="dialog" aria-modal="true" aria-labelledby="modalTitle">
      <header class="modal-header">
        <h2 id="modalTitle">Autenticação Necessária</h2>
        <button class="close-btn" (click)="close()" aria-label="Fechar modal">
          &times;
        </button>
      </header>

      <section class="modal-content">
        <form (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label for="token">Token de Integração</label>
            <div class="input-container">
              <input
                id="token"
                [type]="hide ? 'senha' : 'text'"
                [formControl]="tokenControl"
                required
                aria-describedby="tokenHint tokenError"
                (keydown.escape)="close()"
                autocomplete="one-time-code"
              >
              <button
                type="button"
                class="visibility-btn"
                (click)="toggleVisibility()"
                [attr.aria-label]="hide ? 'Mostrar token' : 'Ocultar token'"
              >
                <span class="material-symbols-outlined">
                  {{ hide ? 'Mostrar token' : 'Ocultar token' }}
                </span>
              </button>
            </div>

            <div
              id="tokenError"
              class="error-message"
              *ngIf="tokenControl.invalid && (tokenControl.dirty || tokenControl.touched)"
            >
              <span *ngIf="tokenControl.hasError('required')">Token é obrigatório!</span>
            </div>

            <p id="tokenHint" class="hint">
              Insira seu token com atenção, garantindo segurança e precisão.
            </p>
          </div>

          <footer class="modal-actions">
            <button
              type="button"
              class="cancel-btn"
              (click)="close()"
              aria-keyshortcuts="Escape"
            >
              Cancelar
            </button>
            <button
              type="submit"
              class="submit-btn"
              [disabled]="tokenControl.invalid"
              aria-live="polite"
            >
              <span class="button-content">
                Validar
                <span class="spinner" *ngIf="isSubmitting"></span>
              </span>
            </button>
          </footer>
        </form>
      </section>
    </div>
  `,
  styles: [`
    :host {
      --primary-color: #6366f1;
      --error-color: #dc2626;
      --success-color: #22c55e;
      --surface-color: #ffffff;
      --text-color: #1f2937;
      --transition-speed: 0.2s;
    }

    .modal-backdrop {
      position: fixed;
      inset: 0;
      background: rgba(0, 0, 0, 0.7);
      backdrop-filter: blur(4px);
      z-index: 999;
    }

    .modal-container {
      position: fixed;
      top: 50%;
      left: 50%;
      transform: translate(-50%, -50%);
      background: var(--surface-color);
      border-radius: 16px;
      box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.25);
      width: min(90vw, 500px);
      z-index: 1000;
      overflow: hidden;
      display: flex;
      flex-direction: column;
      max-height: 90vh;
    }

    .modal-header {
      background: linear-gradient(to right, #50a0f0, #0357ac);
      color: #fff;
      padding: 1.5rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .modal-header h2 {
      margin: 0;
      font-size: 1.5rem;
      font-weight: 600;
      letter-spacing: -0.5px;
    }

    .close-btn {
      background: none;
      border: none;
      color: rgba(255, 255, 255, 0.9);
      font-size: 2rem;
      cursor: pointer;
      padding: 0.5rem;
      transition: opacity var(--transition-speed) ease;
      border-radius: 50%;
      width: 40px;
      height: 40px;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .close-btn:hover {
      background: rgba(255, 255, 255, 0.1);
    }

    .modal-content {
      padding: 2rem;
      display: flex;
      flex-direction: column;
      gap: 1.5rem;
    }

    .form-group {
      display: flex;
      flex-direction: column;
      gap: 0.75rem;
    }

    label {
      font-weight: 500;
      color: var(--text-color);
    }

    .input-container {
      position: relative;
      display: flex;
      align-items: center;
    }

    input {
      width: 100%;
      padding: 0.875rem;
      border: 2px solid #e5e7eb;
      border-radius: 8px;
      font-size: 1rem;
      transition: all var(--transition-speed) ease;
      padding-right: 3rem;
    }

    input:focus {
      border-color: var(--primary-color);
      box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.3);
      outline: none;
    }

    .visibility-btn {
      position: absolute;
      right: 0.75rem;
      background: none;
      border: none;
      cursor: pointer;
      padding: 0.25rem;
      color: var(--text-color);
      opacity: 0.6;
      transition: opacity var(--transition-speed) ease;
    }

    .visibility-btn:hover {
      opacity: 1;
    }

    .error-message {
      color: var(--error-color);
      font-size: 0.875rem;
      font-weight: 500;
      margin-top: 0.25rem;
    }

    .hint {
      font-size: 0.875rem;
      color: #6b7280;
      line-height: 1.5;
      margin: 0;
    }

    .modal-actions {
      display: flex;
      justify-content: flex-end;
      gap: 0.75rem;
      margin-top: 1rem;
    }

    .cancel-btn, .submit-btn {
      padding: 0.75rem 1.5rem;
      border: none;
      border-radius: 8px;
      font-size: 1rem;
      font-weight: 500;
      cursor: pointer;
      transition:
        transform var(--transition-speed) ease,
        opacity var(--transition-speed) ease,
        background-color var(--transition-speed) ease;
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .cancel-btn {
      background: red;
      color: white;
    }

    .cancel-btn:hover {
      background: red;
      transform: translateY(-1px);
    }

    .submit-btn {
      background: linear-gradient(to right, #50a0f0, #0357ac);
      color: white;
    }

    .submit-btn:hover:not(:disabled) {
      background: linear-gradient(to right, #50a0f0, #0357ac);
      transform: translateY(-1px);
    }

    .submit-btn:disabled {
      opacity: 0.7;
      cursor: not-allowed;
    }

    .spinner {
      width: 1rem;
      height: 1rem;
      border: 2px solid rgba(255, 255, 255, 0.3);
      border-top-color: white;
      border-radius: 50%;
      animation: spin 1s linear infinite;
    }

    @keyframes spin {
      to { transform: rotate(360deg); }
    }

    @media (max-width: 480px) {
      .modal-container {
        width: 95vw;
      }

      .modal-actions {
        flex-direction: column-reverse;
      }

      .cancel-btn,
      .submit-btn {
        width: 100%;
        justify-content: center;
      }
    }
  `],
  animations: [
    trigger('modalAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(20px) scale(0.98)' }),
        animate('300ms cubic-bezier(0.4, 0, 0.2, 1)',
          style({ opacity: 1, transform: 'translateY(0) scale(1)' }))
      ]),
      transition(':leave', [
        animate('200ms cubic-bezier(0.4, 0, 0.2, 1)',
          style({ opacity: 0, transform: 'translateY(20px) scale(0.98)' }))
      ])
    ]),
    trigger('backdropFade', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('300ms ease-out', style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('200ms ease-in', style({ opacity: 0 }))
      ])
    ])
  ]
})
export class TokenDialogComponent {
  tokenControl = new FormControl('', [Validators.required]);
  hide = true;
  isSubmitting = false;

  constructor(public dialogRef: MatDialogRef<TokenDialogComponent>) {}

  toggleVisibility(): void {
    this.hide = !this.hide;
  }

  close(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.tokenControl.valid) {
      this.isSubmitting = true;
      setTimeout(() => {
        this.dialogRef.close(this.tokenControl.value);
        this.isSubmitting = false;
      }, 1000);
    }
  }
}
