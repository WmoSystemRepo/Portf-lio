import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RegraModel } from '../../../../shared/models/anypoint/regra.model';
import Swal from 'sweetalert2';
import { DynamicField, FormComponeteCrudComponent } from '../../../../shared/components/anypoint/form-crud/form-crud.component';
import Shepherd from 'shepherd.js';
import { RegraService } from '../../../../core/services/anypoint/auth/regras.service';

@Component({
  selector: 'app-regra-cad-edi',
  imports: [CommonModule, FormComponeteCrudComponent],
  templateUrl: './regra-card-edi.component.html'
})
export class RegraCadEdiComponent implements OnInit {

  //#region Propriedades
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
  //#endregion

  //#region Construtor
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private regraService: RegraService
  ) { }
  //#endregion

  //#region Ciclo de vida
  ngOnInit(): void {
    this.id = this.route.snapshot.paramMap.get('id');
    this.isEditMode = !!this.id;
    this.setupTour();

    this.campos = [
      {
        key: 'regraId',
        label: 'ID da Regra',
        type: 'number',
        required: true,
        hidden: !this.isEditMode
      },
      {
        key: 'regraNome',
        label: 'Nome da Regra',
        type: 'text',
        required: true
      }
    ];

    if (this.isEditMode) {
      this.regraService.getById(this.id!).subscribe({
        next: (data: { id: any; nome: any; }) => {
          this.registroSelecionado = {
            regraId: data.id,
            regraNome: data.nome
          };
        },
        error: () => {
          Swal.fire('Erro', 'Erro ao carregar os dados da regra.', 'error');
          this.router.navigate(['/Regra']);
        }
      });
    } else {
      this.registroSelecionado = {
        regraId: '',
        regraNome: ''
      };
    }
  }
  //#endregion

  //#region Ações CRUD
  salvarNovo(dados: any): void {
    const regra: RegraModel = {
      id: '',
      nome: dados.regraNome
    };

    this.regraService.create(regra).subscribe({
      next: () => {
        Swal.fire('Criado', 'Regra criada com sucesso.', 'success');
        this.router.navigate(['/Regra/Lista']);
      },
      error: () => Swal.fire('Erro', 'Erro ao criar a regra.', 'error')
    });
  }

  salvarEdicao(dados: any): void {
    const regra: RegraModel = {
      id: this.id!,
      nome: dados.regraNome
    };

    this.regraService.update(this.id!, regra).subscribe({
      next: () => {
        Swal.fire('Atualizado', 'Regra atualizada com sucesso.', 'success');
        this.router.navigate(['/Regra/Lista']);
      },
      error: () => Swal.fire('Erro', 'Erro ao atualizar a regra.', 'error')
    });
  }

  voltar(): void {
    this.router.navigate(['/Regra/Lista']);
  }
  //#endregion

  //#region Utilitários
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
      title: 'Formulário de Regra',
      text: 'Utilize este formulário para cadastrar uma nova Regra ou editar os dados de uma já existente. Preencha cada campo com atenção às informações solicitadas.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'campo-nome',
      title: 'Nome da Regra',
      text: 'Informe um nome para a Regra.',
      attachTo: { element: '#regraNome', on: 'bottom' },
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
  //#endregion
}