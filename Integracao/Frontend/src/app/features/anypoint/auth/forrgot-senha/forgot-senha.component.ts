import { Component } from '@angular/core';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '..';

@Component({
    selector: 'app-forgot-senha',
    templateUrl: './forgot-senha.component.html',
    styleUrls: ['./forgot-senha.component.css'],
    imports: [CommonModule, FormsModule]
})
export class ForgotsenhaComponent {
  email: string = '';

  constructor(private accountService: AuthService) {}

  onSubmit(): void {
    this.accountService.forgotsenha(this.email).subscribe(
      response => {
        Swal.fire({
          title: 'Sucesso!',
          text: 'Solicitação de troca de senha enviada com sucesso!',
          icon: 'success',
          confirmButtonText: 'OK'
        });
      },
      () => {
        Swal.fire({
          title: 'Erro!',
          text: 'Houve um problema ao enviar a solicitação de troca de senha. Tente novamente.',
          icon: 'error',
          confirmButtonText: 'OK'
        });
      }
    );
  }
}
