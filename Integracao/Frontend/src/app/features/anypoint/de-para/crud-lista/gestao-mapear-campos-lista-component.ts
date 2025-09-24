import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { catchError, forkJoin, map, Observable, of } from 'rxjs';
import { GestaoMapearCamposModel } from '../../../../shared/models/anypoint/gestao-mapear-campos.model';
import Shepherd from 'shepherd.js';
import { FieldMappingService } from '../../../../core/services/anypoint/mapping/field-mapping.service';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';

@Component({
  selector: 'app-gestao-mapear-campos-lista',
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './gestao-mapear-campos-lista-component.html'
})
export class GestaoMapearCamposListaComponent implements OnInit {

  model: GestaoMapearCamposModel[] = [];

  displayFields: [
    { name: 'nome'; label: 'Nome'; align: 'left'; },
    { name: 'tipo'; label: 'Tipo'; align: 'center'; },
    { name: 'status'; label: 'Status'; align: 'right'; }
  ] | undefined

  modelRefs = this.getModelReferences();
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(
    private service: FieldMappingService,
    private router: Router
  ) { }

  //#region Ciclo de vida
  ngOnInit(): void {
    this.setupTour();
    this.load();
  }
  //#endregion

  //#region Ações CRUD principais
  load(): void {
    debugger

    this.service.ObterListaGestaoMapearCampos().subscribe({
      next: (data: GestaoMapearCamposModel[]) => {
        this.model = data;
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao carregar os de-paras.', 'error');
      }
    });
  }

  add(): void {

    this.router.navigate(['/GestaoMapearCampos/Novo']);
  }

  edit(mapeamento: GestaoMapearCamposModel): void {

    this.router.navigate(['/GestaoMapearCampos/Editar', mapeamento.id]);
  }

  confirmDelete(mapeamento: any): void {
    this.service.deleteMapeamento(mapeamento.id).subscribe({
      next: () => {
        this.load();
        Swal.fire('Sucesso', 'De-para deletado com sucesso.', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao deletar o de-para.', 'error');
      }
    });
  }

  carregarDadosNovamente(): void {
    window.location.reload();
  }
  //#endregion

  //#region Referências (Integrações)
  getModelReferences() {
    return [
      {
        key: 'integracoes',
        field: 'id',
        icon: 'fa-plug',
        label: 'Integrações',
        modalSize: 'xl' as const,
        displayNameField: 'nome',
        displayFields: [
          { name: 'id', label: 'ID' },
          { name: 'nome', label: 'Integração' },
          { name: 'integracaoId', label: 'ID Integração' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensDeparaIntegracao(itemId),
        onSave: (selected: any[] | any, itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarIntegracoes(lista, itemId);
        },
        onRemove: (MapeamentoId: number, IntegracaoId: string) => {
          this.service.ExcluirIntegracaoReferencia(String(MapeamentoId), IntegracaoId).subscribe({
            next: () => console.log(`✔️ Integração desvinculada: ${IntegracaoId}`),
            error: () =>
              Swal.fire('Erro', 'Erro ao remover a vinculação de integração.', 'error')
          });
        },
        modalTitle: `Vincular Integração ao Mapeamento`
      }
    ];
  }

  private vincularStatusEmItensDeparaIntegracao(itemId: number): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaGestaoIntegracoes(),
      associadas: this.service.ObterMapeamentoIntegracoesReferenciaPorMapeamentoId(itemId).pipe(
        catchError(err => {
          return of([]);
        })
      )
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map((integ: { id: any; }) => {
          const relacao = associadas.find((a: any) => a.integracaoId === integ.id);
          return {
            ...integ,
            _checked: !!relacao,
            MapeamentoIntegracaoId: relacao?.id ?? null,
            integracaoId: itemId
          };
        });
      })
    );
  }
  //#endregion

  //#region Salvamento de Referências
  private salvarIntegracoes(selected: any[], itemId: number): void {
    const now = new Date().toISOString();

    const payload = selected.map(item => ({
      mapeamentoId: itemId,
      integracaoId: item.integracaoId ?? item.id,
      dataCriacao: now,
      dataEdicao: now,
      ativo: true
    }));

    this.service.RegistrarDeparaIntegracaoReferencia(payload).subscribe({
      next: (res: any[]) => {
        res.forEach(ret => {
          const item = selected.find(i => (i.id ?? i.integracaoId) === ret.integracaoId);
          if (item) item.mapeamentoIntegracaoId = ret.id;
        });
        Swal.fire('Sucesso', 'Integração registrada com sucesso!', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao registrar integração.', 'error');
      }
    });

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
      title: 'Lista de Mapeamentos',
      text: 'Nesta seção, você visualiza todos os mapeamentos(De/Para) cadastrados no sistema, com suas informações organizadas em uma tabela.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'new-integration-btn',
      title: 'Cadastrar Novo Mapeamento',
      text: 'Clique aqui para acessar a tela de cadastro, onde você poderá preencher o formulário com os dados de um novo mapeamento a ser adicionado ao sistema.',
      attachTo: { element: '.new-integration-btn', on: 'bottom' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'table-wrapper',
      title: 'Tabela de Mapeamentos',
      text: 'Aqui são exibidos os dados dos mapeamentos cadastrados, organizados por colunas para facilitar a visualização e análise.',
      attachTo: { element: '.table-wrapper', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'config-btn',
      title: 'Configurar Colunas da Tabela',
      text: 'Utilize este botão para personalizar quais colunas deseja visualizar na tabela de mapeamentos, de acordo com sua preferência.',
      attachTo: { element: '.config-btn', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'filters-row',
      title: 'Filtros',
      text: 'Aqui você pode aplicar filtros para localizar mapeamentos com base em informações específicas de cada coluna. Utilize o botão "Limpar" para remover todos os filtros e exibir novamente todos os registros.',
      attachTo: { element: '.filters-row', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'edit-btn',
      title: 'Editar Mapeamento',
      text: 'Clique aqui para editar os dados de um mapeamento. Você será redirecionado para uma tela com um formulário preenchido com as informações atuais, pronto para atualização.',
      attachTo: { element: '.edit-btn', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'delete-btn',
      title: 'Excluir Mapeamento',
      text: 'Utilize esta opção para remover um mapeamento do sistema. A exclusão é permanente e exige confirmação antes de ser concluída.',
      attachTo: { element: '.delete-btn', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'detail-btn',
      title: 'Detalhes do Mapeamento',
      text: 'Clique aqui para visualizar, em um modal, informações adicionais do mapeamento que podem ter sido ocultadas na tabela personalizada.',
      attachTo: { element: '.detail-btn', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'reference-btn',
      title: 'Vínculos com Integração',
      text: 'Ao clicar aqui, será exibido um modal com as integrações associadas ao mapeamento. Você poderá adicionar ou remover vínculos com essas integrações diretamente nesta interface.',
      attachTo: { element: '.reference-btn', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });
  }

  startTour(): void {
    this.tour.start();
  }
  //#endregion
}