import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { PendenciasVexpenseModel } from '../../../../shared/models/anypoint/pendencias-vexpense.model';
import { ExclusaoPendenciasModel } from '../../../../shared/models/integracao/VexpesssesBimer/ExclusaoPendencias.model';

interface GetRelatoriosParams {
  pageNumber: number;
  pageSize: number;
  search?: string;
}

@Injectable({
  providedIn: 'root',
})
export class IntegracaoVexpensePendenciasService {
  private apiUrl = environment.api + '/Integracao';

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

  //#region PENDÊNCIAS 
  getPendencias(params: GetRelatoriosParams): Observable<PendenciasVexpenseModel[]> {
    const httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString())
      .set('search', params.search || '');

    return this.http.get<PendenciasVexpenseModel[]>(`${this.apiUrl}/Pendencias`, {
      headers: this.getHeaders(),
      params: httpParams
    });
  }

  excluirPendencias(model: ExclusaoPendenciasModel): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Excluir/Pendencias`, {
      headers: this.getHeaders(),
      body: model
    });
  }
  //#endregion
}
