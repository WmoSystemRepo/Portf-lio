import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { Solicitacao } from '../../../../shared/models/anypoint/solicitacao.model';

@Injectable({
  providedIn: 'root'
})
export class SolicitacoesService {
  private solicitacoesApiUrl = environment.api + '/solicitacoes';

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region AUTH
  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('auth');
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    });
  }
  //#endregion
  
  //#region SOLICITAÇÕES  
  getAll(): Observable<Solicitacao[]> {
    return this.http.get<Solicitacao[]>(`${this.solicitacoesApiUrl}`, { headers: this.getHeaders() });
  }

  aprovar(id: number): Observable<any> {
    return this.http.patch(`${this.solicitacoesApiUrl}/aprovarSolicitacao/${id}`, null, { headers: this.getHeaders() });
  }

  remover(id: number): Observable<any> {
    return this.http.delete(`${this.solicitacoesApiUrl}/${id}`, { headers: this.getHeaders() });
  }
  //#endregion
}
