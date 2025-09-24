import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { RegraModel } from '../../../../shared/models/anypoint/regra.model';
import { Solicitacao } from '../../../../shared/models/anypoint/solicitacao.model';

@Injectable({
  providedIn: 'root'
})
export class SolicitacaoService {
  private apiUrl = `${environment.api}/Regra`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region SOLICITAÇÕES
  getAllSolicitacoes(): Observable<Solicitacao[]> {
    return this.http.get<Solicitacao[]>(this.apiUrl);
  }

  aprovarSolicitacao(id: number): Observable<Solicitacao> {
    throw new Error('Method not implemented.');
  }

  removerSolicitacao(id: number): Observable<void> {
    throw new Error('Method not implemented.');
  }
  //#endregion

  //#region REGRAS
  get(): Observable<RegraModel[]> {
    return this.http.get<RegraModel[]>(this.apiUrl);
  }

  getById(id: string): Observable<RegraModel> {
    return this.http.get<RegraModel>(`${this.apiUrl}/${id}`);
  }

  create(regra: RegraModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/Nova`, regra);
  }

  update(id: string, regra: RegraModel): Observable<any> {
    return this.http.put(`${this.apiUrl}/Editar/${id}`, regra);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  //#endregion
}