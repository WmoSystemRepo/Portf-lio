import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { catchError, forkJoin, map, Observable, of } from 'rxjs';
import Shepherd from 'shepherd.js';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';
import { MenuModel } from '../../../../shared/models/anypoint/menu.model';
import { MenuService } from '../../../../core/services/anypoint/menu/menu.service';

@Component({
  selector: 'app-menu-lista',
  standalone: true,
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './menu-lista.component.html'
})
export class MenuListaComponent implements OnInit {

  model: MenuModel[] = [];

  displayFields:
    | [
      { name: 'nome'; label: 'Nome'; align: 'left' },
      { name: 'tipo'; label: 'Tipo'; align: 'center' },
      { name: 'status'; label: 'Status'; align: 'right' }
    ]
    | undefined;

  modelRefs = this.getModelReferences();
  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  constructor(private service: MenuService, private router: Router) { }

  ngOnInit(): void {
    this.setupTour();
    this.load();
  }

  load(): void {
    this.service.ObterListaMenus().subscribe({
      next: (data: MenuModel[]) => {
        this.model = data;
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao carregar os menus.', 'error');
      }
    });
  }

  add(): void {
    this.router.navigate(['/Menu/Novo']);
  }

  edit(menu: MenuModel): void {
    this.router.navigate(['/Menu/Editar', menu.id?.toString()]);
  }

  confirmDelete(menu: MenuModel): void {
    const id = String(menu.id);
    this.service.DeletarMenu(id).subscribe({
      next: () => {
        Swal.fire('Sucesso', 'Menu deletado com sucesso.', 'success');
        window.location.reload();
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao deletar o menu.', 'error');
      }
    });
  }

  carregarDadosNovamente(): void {
    window.location.reload();
  }

  abrirMenuPai(id: any): void { }

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
      title: 'Lista de Menus',
      text: 'Nesta seção, você visualiza todos os menus cadastrados no sistema, com suas respectivas configurações, organizados em uma tabela.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'new-integration-btn',
      title: 'Cadastrar Novo Menu',
      text: 'Clique aqui para acessar a tela de cadastro, onde você poderá configurar um novo menu, definindo seu nome, rota e permissões associadas.',
      attachTo: { element: '.new-integration-btn', on: 'bottom' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'table-wrapper',
      title: 'Tabela de Menus',
      text: 'Aqui são exibidos os menus cadastrados, com informações como nome, rota e outros dados associados.',
      attachTo: { element: '.table-wrapper', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'config-btn',
      title: 'Configurar Colunas da Tabela',
      text: 'Utilize este botão para personalizar as colunas da tabela, escolhendo quais informações dos menus deseja visualizar.',
      attachTo: { element: '.config-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'filters-row',
      title: 'Filtros',
      text: 'Use os filtros para localizar menus com base em critérios como nome, rota ou Sub-Menu. O botão "Limpar" remove todos os filtros aplicados e exibe novamente a lista completa.',
      attachTo: { element: '.filters-row', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'edit-btn',
      title: 'Editar Menu',
      text: 'Clique aqui para editar as informações de um menu. Você será redirecionado para uma tela com os dados atuais preenchidos, prontos para modificação.',
      attachTo: { element: '.edit-btn', on: 'top' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'delete-btn',
      title: 'Excluir Menu',
      text: 'Utilize esta opção para remover um menu do sistema. A ação é irreversível e requer confirmação.',
      attachTo: { element: '.delete-btn', on: 'right' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'detail-btn',
      title: 'Detalhes do Menu',
      text: 'Clique aqui para visualizar, em um modal, detalhes adicionais do menu que podem estar ocultos na tabela, como ícone, tipo ou hierarquia.',
      attachTo: { element: '.detail-btn', on: 'right' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Próximo', action: this.tour.next },
      ],
    });

    this.tour.addStep({
      id: 'reference-btn',
      title: 'Permissões Relacionadas',
      text: 'Clique aqui para visualizar, em um modal, as regras vinculadas a este menu. Você poderá adicionar ou remover vínculos com regras de acesso conforme necessário.',
      attachTo: { element: '.reference-btn', on: 'right' },
      buttons: [
        { text: 'Voltar', action: this.tour.back },
        { text: 'Finalizar', action: this.tour.complete }
      ],
    });
  }

  startTour(): void {
    this.tour.start();
  }

  getModelReferences() {
    return [
      {
        key: 'regras',
        field: 'id',
        icon: 'fa-sitemap',
        label: 'Regras',
        modalSize: 'xl' as const,
        displayNameField: 'nome',
        displayFields: [
          { name: 'id', label: 'ID' },
          { name: 'nome', label: 'Regra' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensMenuRegra(itemId),
        onSave: (selected: any[], itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarRegras(lista, itemId);
        },
        onRemove: (menuId: number, regraId: string) => {
          this.service.ExcluirMenuRegraReferencia(menuId, regraId).subscribe({
            next: () => console.log(`✔️ Regra desvinculada: ${regraId}`),
            error: () => Swal.fire('Erro', 'Erro ao remover a regra vinculada.', 'error')
          });
        },
        modalTitle: `Vincular Regra ao Menu`
      },
      {
        key: 'usuarios',
        field: 'id',
        icon: 'fa-users',
        label: 'Usuários',
        modalSize: 'xl' as const,
        displayNameField: 'nome',
        displayFields: [
          { name: 'id', label: 'ID' },
          { name: 'userName', label: 'Usuário' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensMenuUsuario(itemId),
        onSave: (selected: any[], itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarUsuario(lista, itemId);
        },
        onRemove: (menuId: number, usuarioId: string) => {
          this.service.ExcluirMenuUsuarioReferencia(menuId, usuarioId).subscribe({
            next: () => console.log(`✔️ Usuário desvinculado: ${usuarioId}`),
            error: () =>
              Swal.fire('Erro', 'Erro ao remover o usuário vinculado.', 'error')
          });
        },
        modalTitle: `Vincular Usuário ao Menu`
      },
      {
        key: 'integracoes',
        field: 'id',
        icon: 'fa-plug',
        label: 'Integrações',
        modalSize: 'xl' as const,
        displayNameField: 'nome',
        displayFields: [
          { name: 'id', label: 'ID' },
          { name: 'nome', label: 'Integração' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensMenuIntegracao(itemId),
        onSave: (selected: any[], itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarIntegracoes(lista, itemId);
        },
        onRemove: (menuId: number, integracaoId: string) => {
          this.service.ExcluirMenuIntegracaoReferencia(menuId, integracaoId).subscribe({
            next: () => console.log(`✔️ Integração desvinculada: ${integracaoId}`),
            error: () =>
              Swal.fire('Erro', 'Erro ao remover a vinculação de integração.', 'error')
          });
        },
        modalTitle: `Vincular Integração ao Menu`
      }
    ];
  }

  private vincularStatusEmItensMenuRegra(itemId: number): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaRegras(),
      associadas: this.service.ObterMenuRegraReferenciaPorMenuId(itemId).pipe(catchError(() => of([])))
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map((regra: { id: any; }) => {
          const relacao = associadas.find((a: any) => a.regraId === regra.id);
          return {
            ...regra,
            _checked: !!relacao,
            menuRegraId: relacao?.id ?? null,
            menuId: itemId
          };
        });
      })
    );
  }

  private vincularStatusEmItensMenuUsuario(itemId: number): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaUsuario(),
      associadas: this.service.ObterMenuUsuarioReferenciaPorMenuId(itemId).pipe(catchError(() => of([])))
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map((user: { id: any; }) => {
          const relacao = associadas.find((a: any) => a.usuarioId === user.id);
          return {
            ...user,
            _checked: !!relacao,
            menuUsuarioId: relacao?.id ?? null,
            menuId: itemId
          };
        });
      })
    );
  }

  private vincularStatusEmItensMenuIntegracao(itemId: number): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaGestaoIntegracoes(),
      associadas: this.service.ObterMenuIntegracoesReferenciaPorMenuId(itemId).pipe(catchError(() => of([])))
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map((integ: { id: any; }) => {
          const relacao = associadas.find((a: any) => a.integracaoId === integ.id);
          return {
            ...integ,
            _checked: !!relacao,
            menuIntegracaoId: relacao?.id ?? null,
            menuId: itemId
          };
        });
      })
    );
  }

  private salvarRegras(selected: any[], itemId: number): void {
    const payload = selected
      .filter(regra => !!regra.id)
      .map(regra => ({
        Id: 0,
        MenuId: itemId,
        RegraId: regra.id,
        Ativo: true,
        DataCriacao: new Date().toISOString()
      }));

    this.service.RegistrarMenuRegra(payload).subscribe({
      error: () => Swal.fire('Erro', 'Erro ao salvar regras.', 'error')
    });
  }

  private salvarUsuario(selected: any[], itemId: number): void {
    const now = new Date().toISOString();

    const payload = selected
      .filter(user => !!user.id)
      .map(user => ({
        MenuId: itemId,
        UsuarioId: user.id,
        Ativo: true,
        DataCriacao: now,
        DataEdicao: now
      }));

    this.service.RegistrarMenuUsuarioReferencia(payload).subscribe({
      next: (res: any) => {
        const lista = Array.isArray(res) ? res : [res];

        lista.forEach(ret => {
          const item = selected.find(p =>
            p.id === ret.PermissaoId || p.id === ret.permissaoId
          );
          if (item) {
            item.menuPermissaoId = ret.Id ?? ret.id;
            item._checked = true;
          }
        });
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao salvar usuário.', 'error');
      }
    });
  }

  private salvarIntegracoes(selected: any[], itemId: number): void {
    const now = new Date().toISOString();

    const payload = selected.map(item => ({
      menuId: itemId,
      integracaoId: item.integracaoId ?? item.id,
      dataCriacao: now,
      dataEdicao: now,
      ativo: true
    }));

    this.service.RegistrarMenuIntegracaoReferencia(payload).subscribe({
      next: (res: any[]) => {
        res.forEach(ret => {
          const item = selected.find(i => (i.id ?? i.integracaoId) === ret.integracaoId);
          if (item) item.menuIntegracaoId = ret.id;
        });
        Swal.fire('Sucesso', 'Integração registrada com sucesso!', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao registrar integração.', 'error');
      }
    });
  }
}
