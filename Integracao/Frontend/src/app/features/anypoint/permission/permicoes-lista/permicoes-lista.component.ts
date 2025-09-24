import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import Shepherd from 'shepherd.js';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';
import { PermicoesModel } from '../../../../shared/models/anypoint/permicoes.model';
import { PermicoesService } from '../../../../core/services/anypoint/auth/permicoes.service';

@Component({
  selector: 'app-permicoes-lista',
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './permicoes-lista.component.html'
})

export class PermicoesListaComponent implements OnInit, OnDestroy {

  integracoes: PermicoesModel[] = [];
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(
    private service: PermicoesService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.load();
  }

  load(): void {
    this.service.get().subscribe(
      (data: PermicoesModel[]) => {
        this.integracoes = data;
      },
      () => {
        Swal.fire('Erro', 'Erro ao carregar as permissões.', 'error');
      }
    );
  }

  ngOnDestroy(): void {
  }

  add(): void {
    this.router.navigate(['/Permissoes/Novo']);
  }

  edit(modelPermicao: PermicoesModel): void {
    this.router.navigate(['/Permicoes/Editar', modelPermicao.id]);
  }

  confirmDelete(integracao: PermicoesModel): void {
    const id = String(integracao.id);
    this.service.delete(id).subscribe(
      () => {
        this.load();
        Swal.fire('Sucesso', 'Permissão deletada com sucesso.', 'success');
      },
      () => Swal.fire('Erro', 'Erro ao deletar a permissão.', 'error')
    );
  }

  carregarDadosNovamente(): void {
    window.location.reload();
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
      id: 'list-title',
      title: 'Lista de Permissões',
      text: 'Nesta seção, você visualiza todas as permissões cadastradas no sistema, organizadas em uma tabela.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'new-integration-btn',
      title: 'Cadastrar Nova Permissão',
      text: 'Clique aqui para acessar a tela de cadastro, onde você poderá criar uma nova permissão no sistema.',
      attachTo: { element: '.new-integration-btn', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'table-wrapper',
      title: 'Tabela de Permissão',
      text: 'Aqui são exibidos as permissões cadastradas no sistema.',
      attachTo: { element: '.table-wrapper', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'config-btn',
      title: 'Configurar Colunas da Tabela',
      text: 'Utilize este botão para personalizar as colunas da tabela, escolhendo quais informações das permissões que deseja visualizar.',
      attachTo: { element: '.config-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'filters-row',
      title: 'Filtros',
      text: 'Use os filtros para localizar a permissão baseada no id, nome ou datas. O botão "Limpar" remove todos os filtros aplicados e exibe novamente a lista completa.',
      attachTo: { element: '.filters-row', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'edit-btn',
      title: 'Editar Permissão',
      text: 'Clique aqui para editar as informações de uma permissão. Você será redirecionado para uma tela com os dados atuais preenchidos, prontos para modificação.',
      attachTo: { element: '.edit-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'delete-btn',
      title: 'Excluir Permissão',
      text: 'Utilize esta opção para remover uma permissão do sistema. A ação é irreversível e requer confirmação.',
      attachTo: { element: '.delete-btn', on: 'right' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'detail-btn',
      title: 'Detalhes da Permissão',
      text: 'Clique aqui para visualizar, em um modal, detalhes adicionais da permissão que podem estar ocultos na tabela.',
      attachTo: { element: '.detail-btn', on: 'right' },
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