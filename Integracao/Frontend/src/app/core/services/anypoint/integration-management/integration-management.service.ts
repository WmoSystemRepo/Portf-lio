import { DebugElement, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';

@Injectable({
  providedIn: 'root'
})
export class IntegrationManagementService {

  private apiUrl = `${environment.api}`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region GESTÃO DE INTEGRAÇÕES 
  get(): Observable<GestaoIntegracaoModel[]> {
    return this.http.get<GestaoIntegracaoModel[]>(`${this.apiUrl}/GestaoIntegracoes/Lista`);
  }

  getById(id: string): Observable<GestaoIntegracaoModel> {
    return this.http.get<GestaoIntegracaoModel>(`${this.apiUrl}/GestaoIntegracoes/${id}`);
  }

  create(modelRetorno: GestaoIntegracaoModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/GestaoIntegracoes/Novo`, modelRetorno);
  }

  update(id: string, model: GestaoIntegracaoModel): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.put(`${this.apiUrl}/GestaoIntegracoes/Editar/${id}`, model, { headers });
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/GestaoIntegracoes/Excluir/${id}`);
  }
  //#endregion
}
