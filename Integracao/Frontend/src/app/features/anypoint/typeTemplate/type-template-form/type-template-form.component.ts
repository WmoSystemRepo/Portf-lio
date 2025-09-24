import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2';
import Shepherd from 'shepherd.js';
import { TipoTemplate } from '../../../../shared/models/anypoint/tipo-template.model';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import { DynamicField, FormComponeteCrudComponent } from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import { TypeTemplateService } from '../../../../core/services/anypoint/templates/type-template.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-type-template-form',
  imports: [CommonModule, FormComponeteCrudComponent],
  templateUrl: './type-template-form.component.html',
})
export class TypeTemplateFormComponent implements OnInit {
  isEditMode = false;
  id: number | null = null;
  registroSelecionado: TipoTemplate | null = null;
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
    private route: ActivatedRoute,
    private router: Router,
    private service: TypeTemplateService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('idReferencia'));
    this.isEditMode = !!this.id;

    this.setupTour();

    this.campos = [
      {
        key: 'id',
        label: 'ID do Template',
        type: 'text',
        required: true,
        hidden: !this.isEditMode,
      },
      {
        key: 'nomeCompleto',
        label: 'Nome Completo',
        type: 'text',
        required: true,
      },
      {
        key: 'sigla',
        label: 'Sigla',
        type: 'text',
        required: true,
      },
      {
        key: 'nomeAbreviado',
        label: 'Nome Abreviado',
        type: 'text',
        required: true,
      },
      {
        key: 'integracaoId',
        label: 'Escolha a Integração',
        type: 'select',
        required: true,
        loadOptions: () =>
          this.service
            .ObterListaGestaoIntegracoes()
            .toPromise()
            .then((x) => x ?? []),
        labelField: 'nome',
        valueField: 'id',
      },
    ];

    if (this.isEditMode) {
      this.service.buscarPorId(this.id!).subscribe({
        next: (data) => {
          this.registroSelecionado = data;
          this.cdr.detectChanges();
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao carregar o TipoTemplate.', 'error');
          this.router.navigate(['/Tipos/Templates/Lista']);
        },
      });
    }
  }

  salvarNovo(dados: TipoTemplate): void {
    const dto: TipoTemplate = {
      nomeCompleto: dados.nomeCompleto,
      sigla: dados.sigla,
      integracaoId: dados.integracaoId,
      nomeAbreviado: dados.nomeAbreviado,
    };

    this.service.criar(dto).subscribe({
      next: () => {
        Swal.fire('Criado', 'TipoTemplate criado com sucesso.', 'success');
        this.router.navigate(['/Tipos/Templates/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao criar o TipoTemplate.', 'error');
      },
    });
  }

  salvarEdicao(dados: TipoTemplate): void {
    const dto: TipoTemplate = {
      id: this.id!,
      nomeCompleto: dados.nomeCompleto,
      sigla: dados.sigla,
      integracaoId: dados.integracaoId,
    };

    this.service.atualizar(this.id!, dto).subscribe({
      next: () => {
        Swal.fire('Atualizado', 'TipoTemplate atualizado com sucesso.', 'success');
        this.router.navigate(['/Tipos/Templates/Lista']);
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao atualizar o TipoTemplate.', 'error');
      },
    });
  }

  cancelar(): void {
    this.router.navigate(['/Tipos/Templates/Lista']);
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
      title: 'Formulário de TipoTemplate',
      text: 'Use este formulário para cadastrar ou editar um TipoTemplate.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-nomeCompleto',
      title: 'Nome Completo',
      text: 'Informe o nome completo do template.',
      attachTo: { element: '#nomeCompleto', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-sigla',
      title: 'Sigla',
      text: 'Informe a sigla do template.',
      attachTo: { element: '#sigla', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'campo-integracao',
      title: 'Integração',
      text: 'Selecione a integração associada ao template.',
      attachTo: { element: '#integracaoId', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Finalizar', action: this.tour.complete },
      ],
    });
  }

  startTour(): void {
    this.tour.start();
  }
}
