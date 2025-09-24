import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import Swal from 'sweetalert2';
import { FormComponeteCrudComponent, DynamicField } from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import Shepherd from 'shepherd.js';
import { IntegrationManagementService } from '../../../../core/services/anypoint/integration-management/integration-management.service';

@Component({
  selector: 'app-gestao-integracoes-cad-edi',
  imports: [CommonModule, ReactiveFormsModule, FormComponeteCrudComponent],
  templateUrl: './gestao-integracao-cad-edi.component.html'
})

export class GestaoIntegracaoCadEdiComponent implements OnInit {
  form!: FormGroup;
  isEditMode = false;
  id: string | null = null;
  registroSelecionado: any = null;

  campos: DynamicField[] = [];
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
    private service: IntegrationManagementService
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.id = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.id;

    this.campos = [
      {
        key: 'id',
        label: 'Id da Integração',
        type: 'text',
        required: true,
        hidden: true
      },
      {
        key: 'nome',
        label: 'Nome da Integração',
        type: 'text',
        required: true,
        hidden: !this.isEditMode
      },
      {
        key: 'projetoOrigem',
        label: 'Projeto de Origem',
        type: 'text',
        required: true
      },
      {
        key: 'projetoDestino',
        label: 'Projeto de Destino',
        type: 'text',
        required: true
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
        next: (data: { [x: string]: any; nome?: any; projetoOrigem?: any; projetoDestino?: any; }) => {
          if (!data.nome?.trim()) {
            data.nome = `${data.projetoOrigem} - ${data.projetoDestino}`;
          }
          this.registroSelecionado = data;
          this.buildForm();
          this.form.patchValue(data);
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao carregar os dados da integração.', 'error');
          this.router.navigate(['/GestaoIntegracoes/Lista']);
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
    dados.nome = `${dados.projetoOrigem} - ${dados.projetoDestino}`;
    dados.dataCriacao = agora;

    const modelIntegracao: any = { ...dados };
    delete modelIntegracao.dataEdicao;
    if (!modelIntegracao.id) delete modelIntegracao.id;

    this.service.create(modelIntegracao).subscribe({
      next: () => {
        Swal.fire('Sucesso', 'Integração criada com sucesso.', 'success');
        this.router.navigate(['/GestaoIntegracoes/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao criar a integração.', 'error');
      }
    });
  }

  salvarEdicao(dados: any): void {
    const agora = new Date().toISOString();
    if (!dados.nome?.trim()) {
      dados.nome = `${dados.projetoOrigem} - ${dados.projetoDestino}`;
    }
    dados.dataEdicao = agora;

    const modelIntegracao: GestaoIntegracaoModel = {
      ...dados,
      id: this.id ?? dados.id
    };

    this.service.update(this.id!, modelIntegracao).subscribe({
      next: () => {
        Swal.fire('Sucesso', 'Integração atualizada com sucesso.', 'success');
        this.router.navigate(['/GestaoIntegracoes/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao atualizar a integração.', 'error');
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
      title: 'Formulário de Integração',
      text: 'Utilize este formulário para cadastrar uma nova integração ou editar os dados de uma já existente. Preencha cada campo com atenção às informações solicitadas.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-nomeOrigem',
      title: 'Nome do projeto de Origem',
      text: 'Informe um nome do projeto de Origem.',
      attachTo: { element: '#projetoOrigem', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-nomeDestino',
      title: 'Nome do projeto de Destino',
      text: 'Informe um nome do projeto de Destino.',
      attachTo: { element: '#projetoDestino', on: 'bottom' },
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
    this.router.navigate(['/GestaoIntegracoes/Lista']);
  }
}