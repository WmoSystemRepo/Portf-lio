import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DynamicField, FormComponeteCrudComponent } from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { GestaoMapearCamposModel } from '../../../../shared/models/anypoint/gestao-mapear-campos.model';
import Shepherd from 'shepherd.js';
import { FieldMappingService } from '../../../../core/services/anypoint/mapping/field-mapping.service';

@Component({
  selector: 'app-gestao-mapear-campos-cred-edi',
  imports: [CommonModule, FormComponeteCrudComponent],
  templateUrl: './gestao-mapear-campos-cred-edi-component.html'
})
export class GestaoMapearCamposFormComponent implements OnInit {
  form!: FormGroup;
  isEditMode: boolean = false;
  id: string | null = null;
  registroSelecionado: any = null;
  campos: DynamicField[] = [];

  mapeamentos: any[] = [];
  isAdmin: boolean = false;
  selectedMapping: any = null;
  showModal: boolean = false;

  sections: any = {
    identificacao: true,
    informacoes: true,
    origem: true,
    destino: true
  };

  newMapping = { campoOrigem: '', campoDestino: '' };
  tour: any = new Shepherd.Tour();

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: FieldMappingService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.id = this.route.snapshot.paramMap.get('idReferencia');
    this.isEditMode = !!this.id;

    this.campos = [
      {
        key: 'id',
        label: 'ID',
        type: 'number',
        required: true,
        hidden: !this.isEditMode
      },
      {
        key: 'idCampos',
        label: 'ID Campos',
        type: 'number',
        required: true
      },
      {
        key: 'tipoExecucao',
        label: 'Tipo de Execução',
        type: 'text',
        required: true
      },
      {
        key: 'integracao',
        label: 'Integração',
        type: 'text',
        required: true
      },
      {
        key: 'nomeMapeamentoOrigem',
        label: 'Nome Mapeamento Origem',
        type: 'text',
        required: true
      },
      {
        key: 'campoOrigem',
        label: 'Campo Origem',
        type: 'text',
        required: true
      },
      {
        key: 'codigoSistemaOrigem',
        label: 'Código Sistema Origem',
        type: 'text',
        required: true
      },
      {
        key: 'valorOrigem',
        label: 'Valor Origem',
        type: 'text',
        required: true
      },
      {
        key: 'nomeMapeamentoDestino',
        label: 'Nome Mapeamento Destino',
        type: 'text',
        required: true
      },
      {
        key: 'campoDestino',
        label: 'Campo Destino',
        type: 'text',
        required: true
      },
      {
        key: 'codigoSistemaDestino',
        label: 'Código Sistema Destino',
        type: 'text',
        required: true
      },
      {
        key: 'valorDestino',
        label: 'Valor Destino',
        type: 'text',
        required: true
      }
    ];

    this.buildForm();

    if (this.isEditMode && this.id) {
      this.service.ObterGestaoMapearCamposPorId(this.id).subscribe({
        next: (data: { [key: string]: any; }) => {
          this.registroSelecionado = data;
          this.buildForm();
          this.form.patchValue(data);
          this.cdr.detectChanges();
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao carregar os dados do mapeamento.', 'error');
          this.router.navigate(['/GestaoMapearCampos/Lista']);
        }
      });
    }
  }

  buildForm(): void {
    const group: Record<string, any> = {};
    this.campos.forEach(field => {
      group[field.key] = field.required ? ['', Validators.required] : [''];
    });
    this.form = this.fb.group(group);
  }

  salvarNovo(dados: GestaoMapearCamposModel): void {
    const mapeamento: GestaoMapearCamposModel = {
      id: 0,
      idCampos: dados.idCampos,
      tipoExecucao: dados.tipoExecucao,
      integracao: dados.integracao,
      nomeMapeamentoOrigem: dados.nomeMapeamentoOrigem,
      campoOrigem: dados.campoOrigem,
      codigoSistemaOrigem: dados.codigoSistemaOrigem,
      valorOrigem: dados.valorOrigem,
      nomeMapeamentoDestino: dados.nomeMapeamentoDestino,
      campoDestino: dados.campoDestino,
      codigoSistemaDestino: dados.codigoSistemaDestino,
      valorDestino: dados.valorDestino,
    };

    this.service.createMapeamento(mapeamento).subscribe({
      next: () => {
        Swal.fire('Criado', 'Mapeamento criado com sucesso.', 'success');
        this.router.navigate(['/GestaoMapearCampos/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao criar o mapeamento.', 'error');
      }
    });
  }

  salvarEdicao(dados: GestaoMapearCamposModel): void {
    const mapeamento: GestaoMapearCamposModel = {
      id: dados.id,
      idCampos: dados.idCampos,
      tipoExecucao: dados.tipoExecucao,
      integracao: dados.integracao,
      nomeMapeamentoOrigem: dados.nomeMapeamentoOrigem,
      campoOrigem: dados.campoOrigem,
      codigoSistemaOrigem: dados.codigoSistemaOrigem,
      valorOrigem: dados.valorOrigem,
      nomeMapeamentoDestino: dados.nomeMapeamentoDestino,
      campoDestino: dados.campoDestino,
      codigoSistemaDestino: dados.codigoSistemaDestino,
      valorDestino: dados.valorDestino,
    };

    this.service.updateMapeamento(this.id!, mapeamento).subscribe({
      next: () => {
        Swal.fire('Atualizado', 'Mapeamento atualizado com sucesso.', 'success');
        this.router.navigate(['/GestaoMapearCampos/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao atualizar o mapeamento.', 'error');
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
      title: 'Formulário de Mapemaento',
      text: 'Utilize este formulário para cadastrar um novo mapemaneto(De/Para) ou atualizar os dados de um já existente. Preencha cada campo com atenção às informações solicitadas.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-idCampos',
      title: 'Id dos Campos',
      text: 'Informe o Id válido.',
      attachTo: { element: '#idCampos', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-tipoExecucao',
      title: 'Tipo de Execução',
      text: 'Defina o tipo de execução que dejesa que esse mapeamento seja. Exemplo: "API" ou "MANUAL".',
      attachTo: { element: '#tipoExecucao', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-integracao',
      title: 'Integração ',
      text: 'Informe a integração cadastrada no sistema que este mapeamento irá usar.',
      attachTo: { element: '#integracao', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-nomeMapeamentoOrigem',
      title: 'Nome Mapemaento de Origem',
      text: 'Informe um nome para o nome do mapeamento de origem.',
      attachTo: { element: '#nomeMapeamentoOrigem', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-campoOrigem',
      title: 'Campo Ogrigem',
      text: 'Informe o campo de origem que será utilizado para esse mapeamento.',
      attachTo: { element: '#campoOrigem', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-codigoSistemaOrigem',
      title: 'Código Sistema de Origem',
      text: 'Informe o código do sistema origem que será utilizado nesse mapeamento.',
      attachTo: { element: '#codigoSistemaOrigem', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-valorOrigem',
      title: 'Valor de Origem',
      text: 'Informe o valor origem. exemplo: "105730".',
      attachTo: { element: '#valorOrigem', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-nomeMapeamentoDestino',
      title: 'Nome do Mapemaento de Destino',
      text: 'Ative esta opção caso o usuário possa visualizar e modificar configurações gerais do sistema.',
      attachTo: { element: '#nomeMapeamentoDestino', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-campoDestino',
      title: 'Campo Ogrigem',
      text: 'Informe o campo de Destino que será utilizado para esse mapeamento.',
      attachTo: { element: '#campoDestino', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-codigoSistemaDestino',
      title: 'Código Sistema de Destino',
      text: 'Informe o código do sistema Destino que será utilizado nesse mapeamento.',
      attachTo: { element: '#codigoSistemaDestino', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-valorDestino',
      title: 'Valor de Destino',
      text: 'Informe o valor Destino. exemplo: "105730".',
      attachTo: { element: '#valorDestino', on: 'bottom' },
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
    this.router.navigate(['/GestaoMapearCampos/Lista']);
  }
}