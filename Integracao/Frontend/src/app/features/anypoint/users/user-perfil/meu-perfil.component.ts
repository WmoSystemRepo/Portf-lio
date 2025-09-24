import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ApplicationUser } from '../../../../shared/models/anypoint/application-user.model';
import { AuthService } from '../../../../core/services/anypoint/auth/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-perfil',
  templateUrl: './meu-perfil.component.html',
  styleUrls: ['./meu-perfil.component.scss'],
  imports: [ReactiveFormsModule, CommonModule]
})
export class PerfilComponent implements OnInit {
  user!: ApplicationUser;
  profileForm!: FormGroup;
  senhaForm!: FormGroup;
  isLoading = false;
  firstLogin: boolean = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.loadUserData();
    this.initForms();
    this.firstLogin = this.authService.firstLogin();
  }

  private loadUserData(): void {
  
    this.isLoading = true;

    this.authService.getUserProfile().subscribe({
      next: (user: ApplicationUser | null) => {
        if (!user) {
          this.snackBar.open('Usuário não encontrado', 'Fechar', { duration: 3000 });
          this.isLoading = false;
          return;
        }

        this.user = user;

        this.profileForm.patchValue({
          id: user.id,
          userName: user.userName,
          email: user.email,
          phoneNumber: user.phoneNumber
        });

        this.isLoading = false;
      },
      error: (err: { error: string }) => {
        const message = err.error || 'Erro ao carregar dados do usuário';
        this.snackBar.open(message, 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }


  private initForms(): void {
    this.profileForm = this.fb.group({
      id: [''],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['']
    });

    this.senhaForm = this.fb.group({
      currentsenha: ['', Validators.required],
      newsenha: ['', [Validators.required, Validators.minLength(8)]],
      confirmsenha: ['', Validators.required]
    }, { validator: this.checksenhas });
  }

  checksenhas(group: FormGroup) {
    const pass = group.get('newsenha')?.value;
    const confirmPass = group.get('confirmsenha')?.value;
    return pass === confirmPass ? null : { notSame: true };
  }

  updateProfile(): void {
    if (this.profileForm.invalid) return;

    this.isLoading = true;
    this.authService.updateUserViaMyProfile(this.profileForm.value).subscribe({
      next: () => {
        this.snackBar.open('Perfil atualizado com sucesso!', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      },
      error: () => {
        this.snackBar.open('Erro ao atualizar perfil', 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }

  changesenha(): void {
    if (this.senhaForm.invalid) return;

    this.isLoading = true;
    this.authService.changesenha(this.senhaForm.value).subscribe({
      next: () => {
        this.snackBar.open('Senha alterada com sucesso!', 'Fechar', { duration: 3000 });
        this.senhaForm.reset();
        this.isLoading = false;
      },
      error: (err: { error: { message: string; }; }) => {
        const message = err.error?.message || 'Erro ao alterar senha';
        this.snackBar.open(message, 'Fechar', { duration: 3000 });
        this.isLoading = false;
      }
    });
  }
}
