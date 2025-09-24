import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { AuthService } from '..';

@Component({
  standalone: true,
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  loading = false;
  isModalOpen = false;
  forgotsenhaEmail = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]+$/)
      ]],
      rememberMe: [false]
    });
  }

  isInvalid(controlName: string): boolean {
    const control = this.loginForm.get(controlName);
    return !!(control && control.invalid && (control.dirty || control.touched));
  }

  login(): void {
  
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.loading = true;

    const payload = {
      email: this.loginForm.value.email,
      senha: this.loginForm.value.senha
    };

    this.authService.login(payload).subscribe({
      next: (response: { accessToken: string; }) => {
        localStorage.setItem('auth', response.accessToken);
        this.router.navigate(['/dashboard']);
      },
      error: () => {
        Swal.fire('Erro', 'Não foi possível fazer login.', 'error');
        this.loading = false;
      }
    });
  }

  openModal(event: Event): void {
    event.preventDefault();
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
  }

  onForgotsenhaSubmit(): void {
    this.authService.forgotsenha(this.forgotsenhaEmail).subscribe({
      next: () => {
        Swal.fire('Info', `Email para redefinição enviado para: ${this.forgotsenhaEmail}`, 'info');
        this.closeModal();
      },
      error: () => {
        Swal.fire('Erro', 'Não foi possível enviar o email de redefinição.', 'error');
      }
    });
  }
}
