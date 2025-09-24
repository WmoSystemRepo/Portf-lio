import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import Shepherd from 'shepherd.js';
import { DynamicField, FormComponeteCrudComponent } from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import { PermicoesService } from '../../../../core/services/anypoint/auth/permicoes.service';
import { PermicoesModel } from '../../../../shared/models/anypoint/permicoes.model';

@Component({
  selector: 'app-permicoes-cad-edi',
  imports: [CommonModule, ReactiveFormsModule, FormComponeteCrudComponent],
  templateUrl: './permicoes-cad-edi.component.html'
})

export class PermicoesCadEdiComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  id: string | null = null;
  registroSelecionado: any = null;
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  campos: DynamicField[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: PermicoesService
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.id = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.id;

    this.campos = [
      {
        key: 'id',
        label: 'Id Permissão',
        type: 'text',
        required: true,
        hidden: true
      },
      {
        key: 'nome',
        label: 'Nome da Permissão',
        type: 'text',
        required: true,
      },
      {
        key: 'dataCriacao',
        label: 'Data de Criação',
        type: 'text',
        required: true,
        hidden: true
      },
      {
        key: 'dataEdicao',
        label: 'Data de Edição',
        type: 'text',
        required: true,
        hidden: true
      }
    ];

    if (this.isEditMode) {
      this.service.getById(this.id!).subscribe({
        next: (data: { [key: string]: any; }) => {
          this.registroSelecionado = data;
          this.buildForm();
          this.form.patchValue(data);
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao carregar os dados da permições.', 'error');
          this.router.navigate(['/Permicoes/Lista']);
        }
      });
    } else {
      this.buildForm();
    }
  }

  buildForm(): void {
    const group: Record<string, any> = {};
    this.campos.forEach(field => {
      group[field.key] = field.required ? ['', Validators.required] : [''];
    });
    this.form = this.fb.group(group);
  }

  salvar(dados: any): void {
    if (this.isEditMode) {
      this.salvarEdicao(dados);
    } else {
      this.salvarNovo(dados);
    }
  }

  salvarNovo(dados: any): void {
    const agora = new Date().toISOString();
    const permicoesModel: PermicoesModel = {
      nome: dados.nome,
      dataCriacao: agora
    };

    this.service.create(permicoesModel).subscribe({
      next: () => {
        Swal.fire('Sucesso', 'Permissão criada com sucesso.', 'success');
        this.router.navigate(['/Permicoes/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao criar a permissão.', 'error');
      }
    });
  }

  salvarEdicao(dados: any): void {
    const agora = new Date().toISOString();
    const dataCriacaoFinal = dados.dataCriacao || this.registroSelecionado?.dataCriacao || agora;

    const modelIntegracao: PermicoesModel = {
      id: this.id ?? dados.id,
      nome: dados.nome,
      dataCriacao: dataCriacaoFinal,
      dataEdicao: agora
    };

    this.service.update(this.id!, modelIntegracao).subscribe({
      next: () => {
        Swal.fire('Sucesso', 'Permissão atualizada com sucesso.', 'success');
        this.router.navigate(['/Permicoes/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao atualizar a permissão.', 'error');
      }
    });
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
      title: 'Formulário de Permissão',
      text: 'Utilize este formulário para cadastrar uma nova permissão ou editar os dados de uma já existente. Preencha cada campo com atenção às informações solicitadas.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-nome',
      title: 'Nome da permissão',
      text: 'Informe um nome para a Permissão.',
      attachTo: { element: '#nome', on: 'bottom' },
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

  voltar(): void {
    this.router.navigate(['/Permicoes/Lista']);
  }
}
