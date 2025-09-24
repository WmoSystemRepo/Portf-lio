import { ViewChild } from '@angular/core';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationUser } from '../../../../shared/models/anypoint/application-user.model';
import { UserEdit } from '../../../../shared/models/anypoint/user-edit.model';
import Shepherd from 'shepherd.js';
import { AuthService } from '../../../../core/services/anypoint/auth/auth.service';
import { DynamicField, FormComponeteCrudComponent } from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import { UserService } from '../../../../core/services/anypoint/users/user.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterUser } from '../../../../shared/models/anypoint/RegisterUser.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-user-form',
  imports: [CommonModule, FormComponeteCrudComponent],
  templateUrl: './user-form.component.html'
})
export class UserFormComponent implements OnInit {
  @ViewChild(FormComponeteCrudComponent) crudForm!: FormComponeteCrudComponent;
  get form(): FormGroup {
    return this.crudForm?.form;
  }

  fields: DynamicField[] = [];
  isEditMode = false;
  userId: string = '';
  registroSelecionado: any = null;
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.userId = this.route.snapshot.paramMap.get('idReferencia') || '';
    this.isEditMode = !!this.userId;

    this.fields = [
      {
        key: 'email',
        label: 'Email',
        type: 'email',
        required: true
      },
      {
        key: 'userName',
        label: 'Nome de Usuário',
        type: 'text',
        required: true
      },
      {
        key: 'password',
        label: 'Senha',
        type: 'text',
        required: true,
        hidden: this.isEditMode
      },
      {
        key: 'phoneNumber',
        label: 'Telefone',
        type: 'text',
        required: true
      },
      {
        key: 'podeLer',
        label: 'Pode Ler',
        type: 'checkbox'
      },
      {
        key: 'podeEscrever',
        label: 'Pode Escrever',
        type: 'checkbox'
      },
      {
        key: 'podeRemover',
        label: 'Pode Remover',
        type: 'checkbox'
      },
      {
        key: 'podeVerConfiguracoes',
        label: 'Pode Ver Configurações',
        type: 'checkbox'
      }
    ];

    if (this.isEditMode) {
      this.userService.getUser(this.userId).subscribe(user => {
        this.registroSelecionado = user;
        this.cdr.detectChanges();
      });
    }
  }

  salvarNovo(user: RegisterUser): void {
    this.userService.createUser(user).subscribe({
      next: () => {
        Swal.fire('Sucesso', 'Usuário criado com sucesso.', 'success');
        this.router.navigate(['/Usuario/Lista']);
      },
      error: (error) => {
        console.log(error)
        if (Array.isArray(error.error)) {
          error.error.forEach((msg: string) => {
            if (msg.toLowerCase().includes('senha')) {
              this.form.get('password')?.setErrors({ serverError: msg });
            }
            if (msg.toLowerCase().includes('email')) {
              this.form.get('email')?.setErrors({ serverError: msg });
            }
            if (msg.toLowerCase().includes('usuário') || msg.toLowerCase().includes('username')) {
              this.form.get('userName')?.setErrors({ serverError: msg });
            }
            if (msg.toLowerCase().includes('telefone') || msg.toLowerCase().includes('phone')) {
              this.form.get('phoneNumber')?.setErrors({ serverError: msg });
            }
          });

          // 🔹 Força o Angular a exibir mensagens de erro
          this.form.markAllAsTouched();
          this.cdr.detectChanges();
        } else {
          Swal.fire('Erro', 'Erro ao criar usuário.', 'error');
        }
      }
    });
  }

  salvarEdicao(user: UserEdit): void {
    user.id = this.userId;
    this.userService.updateUser(user).subscribe(() => {
      this.router.navigate(['/Usuario/Lista']);
    });
  }

  cancelar(): void {
    this.router.navigate(['/Usuario/Lista']);
  }

  setupTour(): void {
    this.tour = new Shepherd.Tour({
      defaultStepOptions: {
        scrollTo: true,
        cancelIcon: { enabled: true },
        classes: 'shadow-md bg-white',
      },
      useModalOverlay: true,
    });

    this.tour.addStep({
      id: 'form-title',
      title: 'Formulário de Usuário',
      text: 'Utilize este formulário para cadastrar um novo usuário ou atualizar os dados de um já existente. Preencha cada campo com atenção às informações solicitadas.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-email',
      title: 'E-mail do Usuário',
      text: 'Informe um endereço de e-mail válido. Esse será o principal meio de contato e também poderá ser utilizado para login (exemplo: nome@dominio.com).',
      attachTo: { element: '#email', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-username',
      title: 'Nome de Usuário',
      text: 'Defina um nome de usuário único, que será utilizado para identificação no sistema. Evite espaços e caracteres especiais.',
      attachTo: { element: '#userName', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-password',
      title: 'Senha de Acesso',
      text: 'Crie uma senha segura com no mínimo 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais. Essa senha será usada para login no sistema.',
      attachTo: { element: '#password', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-telefone',
      title: 'Telefone de Contato',
      text: 'Informe o número de telefone do usuário, incluindo DDD. Este dado pode ser utilizado para contato ou validações futuras.',
      attachTo: { element: '#phoneNumber', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'podeLer',
      title: 'Permissão: Visualizar Dados',
      text: 'Habilite esta opção caso o usuário deva ter acesso de leitura às informações do sistema, sem possibilidade de alteração.',
      attachTo: { element: '#podeLer', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'podeEscrever',
      title: 'Permissão: Editar Dados',
      text: 'Selecione esta opção se o usuário puder criar ou modificar informações dentro do sistema.',
      attachTo: { element: '#podeEscrever', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'podeRemover',
      title: 'Permissão: Excluir Dados',
      text: 'Marque esta opção para conceder ao usuário permissão para excluir registros do sistema.',
      attachTo: { element: '#podeRemover', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'podeVerConfiguracoes',
      title: 'Permissão: Acesso às Configurações',
      text: 'Ative esta opção caso o usuário possa visualizar e modificar configurações gerais do sistema.',
      attachTo: { element: '#podeVerConfiguracoes', on: 'left' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'acoes-formulario',
      title: 'Ações do Formulário',
      text: 'Após preencher todos os campos, clique em “Salvar” para concluir o cadastro ou em “Cancelar” para descartar as alterações.',
      attachTo: { element: '.button-group', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Finalizar', action: this.tour.complete },
      ],
    });
  }

  startTour(): void {
    this.tour.start();
  }

  private validarSenha(senha: string): boolean {
    const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{8,}$/;
    return regex.test(senha);
  }
}