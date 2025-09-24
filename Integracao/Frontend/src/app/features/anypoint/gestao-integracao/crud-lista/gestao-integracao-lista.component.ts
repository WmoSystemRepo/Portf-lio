import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import Shepherd from 'shepherd.js';
import { IntegrationManagementService } from '../../../../core/services/anypoint/integration-management/integration-management.service';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';

@Component({
  selector: 'app-gestao-integracao-lista',
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './gestao-integracao-lista.component.html',
  styleUrls: ['./gestao-integracao-lista.component.css']
})

export class GestaoIntegracaoListaComponent implements OnInit, OnDestroy {

  integracoes: GestaoIntegracaoModel[] = [];
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(
    private service: IntegrationManagementService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.load();
  }

  load(): void {
    this.service.get().subscribe(
      (data: GestaoIntegracaoModel[]) => {
        this.integracoes = data;
      },
      () => {
        Swal.fire('Erro', 'Erro ao carregar as integrações.', 'error');
      }
    );
  }

  ngOnDestroy(): void {
  }

  add(): void {
    this.router.navigate(['/GestaoIntegracoes/Novo']);
  }

  edit(integracao: GestaoIntegracaoModel): void {
    this.router.navigate(['/GestaoIntegracoes/Editar', integracao.id]);
  }

  confirmDelete(integracao: GestaoIntegracaoModel): void {
    const id = String(integracao.id);
    this.service.delete(id).subscribe(
      () => {
        this.load();
        Swal.fire('Sucesso', 'Regra deletada com sucesso.', 'success');
      },
      () => Swal.fire('Erro', 'Erro ao deletar a regra.', 'error')
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
      title: 'Lista de Integrações',
      text: 'Nesta seção, você visualiza todas as integrações cadastradas no sistema, organizadas em uma tabela.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'new-integration-btn',
      title: 'Cadastrar Nova Integração',
      text: 'Clique aqui para acessar a tela de cadastro, onde você poderá criar uma nova integração no sistema.',
      attachTo: { element: '.new-integration-btn', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'table-wrapper',
      title: 'Tabela de Integração',
      text: 'Aqui são exibidos as integrações cadastradas no sistema.',
      attachTo: { element: '.table-wrapper', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'config-btn',
      title: 'Configurar Colunas da Tabela',
      text: 'Utilize este botão para personalizar as colunas da tabela, escolhendo quais informações das integrações que deseja visualizar.',
      attachTo: { element: '.config-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'filters-row',
      title: 'Filtros',
      text: 'Use os filtros para localizar a integração baseada no id, nome ou datas. O botão "Limpar" remove todos os filtros aplicados e exibe novamente a lista completa.',
      attachTo: { element: '.filters-row', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'edit-btn',
      title: 'Editar integração',
      text: 'Clique aqui para editar as informações de uma integração. Você será redirecionado para uma tela com os dados atuais preenchidos, prontos para modificação.',
      attachTo: { element: '.edit-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'delete-btn',
      title: 'Excluir integração',
      text: 'Utilize esta opção para remover uma integração do sistema. A ação é irreversível e requer confirmação.',
      attachTo: { element: '.delete-btn', on: 'right' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'detail-btn',
      title: 'Detalhes da integração',
      text: 'Clique aqui para visualizar, em um modal, detalhes adicionais da integração que podem estar ocultos na tabela.',
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