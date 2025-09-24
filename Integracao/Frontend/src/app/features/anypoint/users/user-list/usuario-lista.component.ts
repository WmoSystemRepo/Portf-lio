import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { catchError, forkJoin, map, Observable, of } from 'rxjs';
import { UsuarioModel } from '../../../../shared/models/anypoint/usuario.model';
import { UsuarioService } from '../../../../core/services/anypoint/users/usuario.service';
import Shepherd from 'shepherd.js';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';

@Component({
  selector: 'app-usuario-lista',
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './usuario-lista.component.html'
})
export class UsuarioListaComponent implements OnInit {

  //#region Propriedades
  model: UsuarioModel[] = [];

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

  //#endregion

  //#region Construtor
  constructor(
    private service: UsuarioService,
    private router: Router
  ) { }
  //#endregion

  //#region Ciclo de vida
  ngOnInit(): void {
    this.setupTour();
    this.load();
  }
  //#endregion

  //#region Ações CRUD principais
  load(): void {
    this.service.ObterListaUsuario().subscribe({
      next: (data: UsuarioModel[]) => {
        this.model = data;
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao carregar os usuarios.', 'error');
      }
    });
  }

  add(): void {
    this.router.navigate(['/Usuario/Novo']);
  }

  edit(model: UsuarioModel): void {
    this.router.navigate(['/Usuario/Editar', model.id]);
  }

  confirmDelete(usuario: any): void {
    this.service.DeletarUsuario(usuario.id).subscribe({
      next: () => {
        this.load();
        Swal.fire('Sucesso', 'Usuario deletado com sucesso.', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao deletar o Usuario.', 'error');
      }
    });
  }

  carregarDadosNovamente(): void {
    window.location.reload();
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
      title: 'Lista de Usuários',
      text: 'Nesta seção, você visualiza todas as pessoas cadastradas no sistema, com suas principais informações organizadas em uma tabela.',
      attachTo: { element: '.app-container', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'new-integration-btn',
      title: 'Cadastrar Novo Usuário',
      text: 'Clique aqui para acessar a tela de cadastro, onde você poderá preencher o formulário com os dados de um novo usuário a ser adicionado ao sistema.',
      attachTo: { element: '.new-integration-btn', on: 'bottom' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'table-wrapper',
      title: 'Tabela de Usuários',
      text: 'Aqui são exibidos os dados dos usuários cadastrados, organizados por colunas para facilitar a visualização e análise.',
      attachTo: { element: '.table-wrapper', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'config-btn',
      title: 'Configurar Colunas da Tabela',
      text: 'Utilize este botão para personalizar quais colunas deseja visualizar na tabela de usuários, de acordo com sua preferência.',
      attachTo: { element: '.config-btn', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'filters-row',
      title: 'Filtros',
      text: 'Aqui você pode aplicar filtros para localizar usuários com base em informações específicas de cada coluna. Utilize o botão "Limpar" para remover todos os filtros e exibir novamente todos os registros.',
      attachTo: { element: '.filters-row', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'edit-btn',
      title: 'Editar Usuário',
      text: 'Clique aqui para editar os dados de um usuário. Você será redirecionado para uma tela com um formulário preenchido com as informações atuais, pronto para atualização.',
      attachTo: { element: '.edit-btn', on: 'top' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'delete-btn',
      title: 'Excluir Usuário',
      text: 'Utilize esta opção para remover um usuário do sistema. A exclusão é permanente e exige confirmação antes de ser concluída.',
      attachTo: { element: '.delete-btn', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'detail-btn',
      title: 'Detalhes do Usuário',
      text: 'Clique aqui para visualizar, em um modal, informações adicionais do usuário que podem ter sido ocultadas na tabela personalizada.',
      attachTo: { element: '.detail-btn', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });

    this.tour.addStep({
      id: 'reference-btn',
      title: 'Vínculos com Regras',
      text: 'Ao clicar aqui, será exibido um modal com as regras associadas ao usuário. Você poderá adicionar ou remover vínculos com essas regras diretamente nesta interface.',
      attachTo: { element: '.reference-btn', on: 'right' },
      buttons: [{ text: 'Próximo', action: this.tour.next }],
    });
  }

  startTour(): void {
    this.tour.start();
  }
  //#endregion

  //#region Referências
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
          { name: 'nome', label: 'Regra' },
          { name: 'usuarioId', label: 'ID Usuário' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensUsuarioRegra(itemId),
        onSave: (selected: any[] | any, itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarRegra(lista, itemId);
        },
        onRemove: (UsuarioId: number, RegraId: string) => {
          this.service.ExcluirUsuarioRegraReferencia(String(UsuarioId), RegraId).subscribe({
            next: () => console.log(`✔️ Regra desvinculada: ${UsuarioId}`),
            error: () =>
              Swal.fire('Erro', 'Erro ao remover a regra vinculada.', 'error')
          });
        },
        modalTitle: `Vincular Regra ao Usuário`
      },
      {
        key: 'Permissoes',
        field: 'id',
        icon: 'fa-lock',
        label: 'Permissões',
        modalSize: 'xl' as const,
        displayNameField: 'nome',
        displayFields: [
          { name: 'id', label: 'ID' },
          { name: 'nome', label: 'Permissões' },
          { name: 'usuarioId', label: 'ID Usuário' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensUsuarioPermissao(itemId),
        onSave: (selected: any[] | any, itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarPermissao(lista, itemId);
        },
        onRemove: (UsuarioId: number, PermissaoId: string) => {
          this.service.ExcluirUsuarioPermissaoReferencia(String(UsuarioId), PermissaoId).subscribe({
            next: () => console.log(`✔️ Integração desvinculada: ${UsuarioId}`),
            error: () =>
              Swal.fire('Erro', 'Erro ao remover a vinculação de integração.', 'error')
          });
        },
        modalTitle: `Vincular Permissão ao Usuário`
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
          { name: 'nome', label: 'Integração' },
          { name: 'usuarioId', label: 'ID Usuário' }
        ],
        fetchFn: (itemId: number) => this.vincularStatusEmItensUsuarioIntegracao(itemId.toString()),
        onSave: (selected: any[] | any, itemId: number) => {
          const lista = Array.isArray(selected) ? selected : [selected];
          this.salvarIntegracoes(lista, itemId);
        },
        onRemove: (UsuarioId: number, IntegracaoId: string) => {
          this.service.ExcluirUsuarioIntegracaoReferencia(String(UsuarioId), IntegracaoId).subscribe({
            next: () => console.log(`✔️ Integração desvinculada: ${IntegracaoId}`),
            error: () =>
              Swal.fire('Erro', 'Erro ao remover a vinculação de integração.', 'error')
          });
        },
        modalTitle: `Vincular Integração ao Usuário`
      }
    ];
  }

  private vincularStatusEmItensUsuarioRegra(itemId: number): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaRegras(),
      associadas: this.service.ObterUsuarioRegrasReferenciaPorUsuarioId(itemId).pipe(
        catchError(() => of([]))
      )
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map(regra => {
          const relacao = associadas.find((a: any) => a.regraId === regra.id);
          return {
            ...regra,
            _checked: !!relacao,
            usuarioRegraId: relacao?.id ?? null,
            usuarioId: itemId
          };
        });
      })
    );
  }

  private vincularStatusEmItensUsuarioPermissao(itemId: number): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaPermissoes(),
      associadas: this.service.ObterUsuarioPermissoesReferenciaPorUsuarioId(itemId.toString()).pipe(
        catchError(() => of([]))
      )
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map(perm => {
          const relacao = associadas.find((a: any) => a.permissaoId === perm.id);
          return {
            ...perm,
            _checked: !!relacao,
            usuarioPermissaoId: relacao?.id ?? null,
            usuarioId: itemId
          };
        });
      })
    );
  }

  private vincularStatusEmItensUsuarioIntegracao(itemId: string): Observable<any[]> {
    return forkJoin({
      todas: this.service.ObterListaGestaoIntegracoes(),
      associadas: this.service.ObterUsuarioIntegracoesReferenciaPorUsuarioId(itemId).pipe(
        catchError(() => of([]))
      )
    }).pipe(
      map(({ todas, associadas }) => {
        return todas.map(integ => {
          const relacao = associadas.find((a: any) => a.integracaoId === integ.id);
          return {
            ...integ,
            _checked: !!relacao,
            usuarioIntegracaoId: relacao?.id ?? null,
            usuarioId: itemId
          };
        });
      })
    );
  }
  //#endregion

  //#region Salvamento de Referências
  private salvarRegra(selected: any[], itemId: number): void {
    const now = new Date().toISOString();

    const payload = selected.map(item => ({
      usuarioId: itemId,
      regraId: item.regraId ?? item.id,
      dataCriacao: now,
      dataEdicao: now,
      ativo: true
    }));

    this.service.RegistrarUsuarioRegraReferencia(payload).subscribe({
      next: (res: any[]) => {
        res.forEach(ret => {
          const item = selected.find(i => (i.id ?? i.regraId) === ret.regraId);
          if (item) item.usuarioRegraId = ret.id;
        });
        Swal.fire('Sucesso', 'Regra registrada com sucesso!', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao registrar regra.', 'error');
      }
    });
  }

  private salvarPermissao(selected: any[], itemId: number): void {
    const now = new Date().toISOString();

    const payload = selected.map(item => ({
      usuarioId: itemId,
      permissaoId: item.permissaoId ?? item.id,
      dataCriacao: now,
      dataEdicao: now,
      ativo: true
    }));

    this.service.RegistrarUsuarioPermissaoReferencia(payload).subscribe({
      next: (res: any[]) => {
        res.forEach(ret => {
          const item = selected.find(i => (i.id ?? i.permissaoId) === ret.permissaoId);
          if (item) item.usuarioPermissaoId = ret.id;
        });
        Swal.fire('Sucesso', 'Permissão registrada com sucesso!', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao registrar integração.', 'error');
      }
    });
  }

  private salvarIntegracoes(selected: any[], itemId: number): void {
    const now = new Date().toISOString();

    const payload = selected.map(item => ({
      usuarioId: itemId,
      integracaoId: item.integracaoId ?? item.id,
      dataCriacao: now,
      dataEdicao: now,
      ativo: true
    }));

    this.service.RegistrarUsuarioIntegracaoReferencia(payload).subscribe({
      next: (res: any[]) => {
        res.forEach(ret => {
          const item = selected.find(i => (i.id ?? i.integracaoId) === ret.integracaoId);
          if (item) item.usuarioIntegracaoId = ret.id;
        });
        Swal.fire('Sucesso', 'Integração registrada com sucesso!', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao registrar integração.', 'error');
      }
    });
  }
  //#endregion
}