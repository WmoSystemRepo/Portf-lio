import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { UsuarioModel } from '../../../../shared/models/anypoint/usuario.model';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import { PermicoesModel } from '../../../../shared/models/anypoint/permicoes.model';
import { RegraModel } from '../../../../shared/models/anypoint/regra.model';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {
  private apiUrl = `${environment.api}/Usuario`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region USUÁRIO
  ObterListaUsuario(): Observable<UsuarioModel[]> {
    return this.http.get<UsuarioModel[]>(`${this.apiUrl}/Lista`);
  }

  ObterUsuarioPorId(id: string): Observable<UsuarioModel> {
    return this.http.get<UsuarioModel>(`${this.apiUrl}/${id}`);
  }

  RegistrarUsuario(data: UsuarioModel): Observable<UsuarioModel> {
    return this.http.post<UsuarioModel>(`${this.apiUrl}/Nova`, data);
  }

  EditarUsuario(id: string, data: UsuarioModel): Observable<UsuarioModel> {
    return this.http.put<UsuarioModel>(`${this.apiUrl}/Editar`, data);
  }

  DeletarUsuario(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  //#endregion

  //#region USUÁRIO REGRAS
  ObterListaRegras(): Observable<RegraModel[]> {
    return this.http.get<RegraModel[]>(`${environment.api}/Regra/Lista`);
  }

  ObterUsuarioRegrasReferenciaPorUsuarioId(usuarioId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Regra/IdUsuario/${usuarioId}`);
  }

  RegistrarUsuarioRegraReferencia(payload: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/Regra/Novo`, payload);
  }

  ExcluirUsuarioRegraReferencia(usuarioId: string, regraId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Regra/Excluir/Referencia/${usuarioId}/${regraId}`);
  }

  ObterUsuarioRegraPorIdReferencia(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/Regra/IdReferencia/${id}`);
  }
  //#endregion

  //#region USUÁRIO PERMISSÕES
  ObterListaPermissoes(): Observable<PermicoesModel[]> {
    return this.http.get<PermicoesModel[]>(`${environment.api}/Permicoes/Lista`);
  }

  ObterUsuarioPermissoesReferenciaPorUsuarioId(usuarioId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Permissao/IdUsuario/${usuarioId}`);
  }

  RegistrarUsuarioPermissaoReferencia(payload: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/Permissao/Novo`, payload);
  }

  AtualizarUsuarioPermissao(id: string, payload: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Permissao/${id}`, payload);
  }

  ExcluirUsuarioPermissaoReferencia(usuarioId: string, permissaoId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Permissao/Excluir/Referencia/${usuarioId}/${permissaoId}`);
  }

  ObterUsuarioPermissaoPorIdReferencia(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/Permissao/IdReferencia/${id}`);
  }
  //#endregion

  //#region USUÁRIO INTEGRAÇÕES
  ObterListaGestaoIntegracoes(): Observable<GestaoIntegracaoModel[]> {
    return this.http.get<GestaoIntegracaoModel[]>(`${environment.api}/GestaoIntegracoes/Lista`);
  }

  ObterUsuarioIntegracoesReferenciaPorUsuarioId(usuarioId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Integracao/IdUsuario/${usuarioId}`);
  }

  RegistrarUsuarioIntegracaoReferencia(payload: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/Integracao/Novo`, payload);
  }

  AtualizarUsuarioIntegracao(id: string, payload: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/Integracao/${id}`, payload);
  }

  ExcluirUsuarioIntegracaoReferencia(usuarioId: string, integracaoId: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Integracao/Excluir/Referencia/${usuarioId}/${integracaoId}`);
  }

  ObterUsuarioIntegracaoPorIdReferencia(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/Integracao/IdReferencia/${id}`);
  }
  //#endregion
}