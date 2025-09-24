import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import Shepherd from 'shepherd.js';
import { RegraModel } from '../../../../shared/models/anypoint/regra.model';
import { RegraService } from '../../../../core/services/anypoint/auth/regras.service';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';

@Component({
  selector: 'app-regra-lista',
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './regra-lista.component.html',
  styleUrls: ['./regra-lista.component.css']
})
export class RegraListaComponent implements OnInit {
  model: RegraModel[] = [];
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(
    private service: RegraService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.setupTour();
    this.load();
  }

  load(): void {
    this.service.get().subscribe(
      (data: RegraModel[]) => {
        this.model = data;
      },
      () => {
        Swal.fire('Erro', 'Erro ao carregar as regras.', 'error');
      }
    );
  }

  add(): void {
    this.router.navigate(['/Regras/Nova']);
  }

  edit(regra: RegraModel): void {
    this.router.navigate(['/Regra/Editar', regra.id]);
  }

  confirmDelete(regra: RegraModel): void {
    const id = String(regra.id);
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
      title: 'Lista de Regras',
      text: 'Nesta seção, você visualiza todas as regras cadastradas no sistema, organizadas em uma tabela.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'new-integration-btn',
      title: 'Cadastrar Nova Regra',
      text: 'Clique aqui para acessar a tela de cadastro, onde você poderá criar uma nova regra no sistema.',
      attachTo: { element: '.new-integration-btn', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'table-wrapper',
      title: 'Tabela de Regra',
      text: 'Aqui são exibidos as regras cadastradas no sistema.',
      attachTo: { element: '.table-wrapper', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'config-btn',
      title: 'Configurar Colunas da Tabela',
      text: 'Utilize este botão para personalizar as colunas da tabela, escolhendo quais informações das regras que deseja visualizar.',
      attachTo: { element: '.config-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'filters-row',
      title: 'Filtros',
      text: 'Use os filtros para localizar a regra baseada no id, nome ou datas. O botão "Limpar" remove todos os filtros aplicados e exibe novamente a lista completa.',
      attachTo: { element: '.filters-row', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'edit-btn',
      title: 'Editar Regra',
      text: 'Clique aqui para editar as informações de uma regra. Você será redirecionado para uma tela com os dados atuais preenchidos, prontos para modificação.',
      attachTo: { element: '.edit-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'delete-btn',
      title: 'Excluir Regra',
      text: 'Utilize esta opção para remover uma regra do sistema. A ação é irreversível e requer confirmação.',
      attachTo: { element: '.delete-btn', on: 'right' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'detail-btn',
      title: 'Detalhes da Regra',
      text: 'Clique aqui para visualizar, em um modal, detalhes adicionais da regra que podem estar ocultos na tabela.',
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
