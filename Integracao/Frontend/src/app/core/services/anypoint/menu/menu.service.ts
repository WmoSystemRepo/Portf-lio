import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { MenuModel } from '../../../../shared/models/anypoint/menu.model';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import { PermicoesModel } from '../../../../shared/models/anypoint/permicoes.model';
import { RegraModel } from '../../../../shared/models/anypoint/regra.model';
import { UsuarioModel } from '../../../../shared/models/anypoint/usuario.model';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private apiUrl = `${environment.api}/Menu`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region MENU
  ObterListaMenus(): Observable<MenuModel[]> {
    return this.http.get<MenuModel[]>(`${this.apiUrl}/Lista`);
  }

  ObterMenuPorId(id: string): Observable<MenuModel> {
    return this.http.get<MenuModel>(`${this.apiUrl}/IdMenu/${id}`);
  }

  RegistrarMenu(model: MenuModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/Novo`, model);
  }

  AtualizarMenu(id: string, model: MenuModel): Observable<any> {
    return this.http.put(`${this.apiUrl}/Editar/${id}`, model, {
      headers: new HttpHeaders({ 'Content-Type': 'application/json' })
    });
  }

  DeletarMenu(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Excluir/${id}`);
  }

  ObterListsMenusPrincipal(): Observable<MenuModel[]> {
    return this.ObterListaMenus().pipe(
      map(menus => menus.filter(menu => menu.ehMenuPrincipal === true))
    );
  }
  //#endregion

  //#region MENU REGRAS
  ObterListaRegras(): Observable<RegraModel[]> {
    return this.http.get<RegraModel[]>(`${environment.api}/Regra/Lista`);
  }

  RegistrarMenuRegra(payload: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Regra/Novo`, payload);
  }

  ObterMenuRegraReferenciaPorMenuId(menuId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Regra/IdMenu/${menuId}`);
  }

  ExcluirMenuRegraReferencia(menuId: number, regraId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Regra/Excluir/Referencia/${menuId}/${regraId}`);
  }
  //#endregion

  //#region MENU USUÁRIO
  ObterListaUsuario(): Observable<UsuarioModel[]> {
    return this.http.get<UsuarioModel[]>(`${environment.api}/Usuario/Lista`);
  }

  ObterMenuUsuarioReferenciaPorMenuId(menuId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Usuario/IdMenu/${menuId}`);
  }

  RegistrarMenuUsuarioReferencia(payload: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/Usuario/Novo`, payload);
  }

  AtualizarMenuUsuario(id: string, payload: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuario/Editar/${id}`, payload);
  }

  ExcluirMenuUsuarioReferencia(menuId: number, usuarioId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Usuario/Excluir/Referencia/${menuId}/${usuarioId}`);
  }
  //#endregion

  //#region MENU INTEGRAÇÕES
  ObterListaGestaoIntegracoes(): Observable<GestaoIntegracaoModel[]> {
    return this.http.get<GestaoIntegracaoModel[]>(`${environment.api}/GestaoIntegracoes/Lista`);
  }

  ObterMenuIntegracoesReferenciaPorMenuId(menuId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Integracao/IdMenu/${menuId}`);
  }

  RegistrarMenuIntegracaoReferencia(payload: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/Integracao/Novo`, payload);
  }

  ExcluirMenuIntegracaoReferencia(menuId: number, integracaoId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Integracao/Excluir/Referencia/${menuId}/${integracaoId}`);
  }

  ObterMenuIntegracaoPorIdReferencia(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/Integracao/IdReferencia/${id}`);
  }
  //#endregion
}
