import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { PendenciasVexpenseModel } from '../../../../shared/models/anypoint/pendencias-vexpense.model';

@Injectable({ providedIn: 'root' })
export class IntegracaoVexpensesXBimmerService {
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

  //#region INSERÇÃO DE TÍTULOS  
  inserirTitulosManual(ids: number[]): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Inserir/Titulos/Manual`, ids, {
      headers: this.getHeaders()
    });
  }

  inserirTitulosEmMassa(): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Inserir/Titulos/EmMassa`, {}, {
      headers: this.getHeaders()
    });
  }
  //#endregion

  //#region PENDÊNCIAS 
  ObterPendenciasIntegracaoVexpense(params: {
    pageNumber: number;
    pageSize: number;
    status?: string;
    search?: string;
  }): Observable<PendenciasVexpenseModel[]> {
    const httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString())
      .set('status', params.status ?? '')
      .set('search', params.search ?? '');

    return this.http.get<PendenciasVexpenseModel[]>(`${this.apiUrl}/Pendencias`, {
      headers: this.getHeaders(),
      params: httpParams
    });
  }
  //#endregion

  //#region ERROS 
  buscarTentativasInsercaoErroIntegracaoBimmer(params: {
    pageNumber: number;
    pageSize: number;
    search?: string;
  }): Observable<any> {
    const httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString())
      .set('search', params.search ?? '');

    return this.http.get<any>(`${this.apiUrl}/Lista/Erros`, {
      headers: this.getHeaders(),
      params: httpParams
    });
  }
  //#endregion

  //#region SUCESSO 
  buscarInsercaoComSucessoIntegracaoBimmer(params: {
    pageNumber: number;
    pageSize: number;
    search?: string;
  }): Observable<any> {
    const httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString())
      .set('search', params.search ?? '');

    return this.http.get<any>(`${this.apiUrl}/Sucesso`, {
      headers: this.getHeaders(),
      params: httpParams
    });
  }
  //#endregion
}
