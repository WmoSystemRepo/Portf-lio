import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';

//#region Interfaces
interface GetRelatoriosParams {
  include: any;
  searchJoin: any;
  searchField: any;
  pageNumber: number;
  pageSize: number;
  status?: string;
  search?: string;
}

interface ReportCounts {
  totalGeral: number;
  totalPago: number;
  totalPendencias: number;
  totalPendenciasMoeda: number;
  totalErros: number;
  totalAprovados: number;
  totalAbertos: number
  totalReabertos: number;
  totalReprovados: number;
  totalEnviados: number;
  totalSucessos: number;
}
//#endregion

@Injectable({
  providedIn: 'root'
})
export class RelatoriosService {
  private apiUrl = environment.api + '/Integracao/Vexpesses';
  private apiUrlIntegracaoBimmer = environment.api + '/Integracao';

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

  //#region RELATÓRIOS 
  getRelatorios(params: any) {
    let httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString());

    if (params.status) httpParams = httpParams.set('status', params.status);
    if (params.search) httpParams = httpParams.set('search', params.search);
    if (params.searchField) httpParams = httpParams.set('searchField', params.searchField);
    if (params.searchJoin) httpParams = httpParams.set('searchJoin', params.searchJoin);
    if (params.include) httpParams = httpParams.set('include', params.include);

    return this.http.get<{ reports: any[]; totalItems: number }>(
      this.apiUrl + '/Listar/Titulos',
      { headers: this.getHeaders(), params: httpParams }
    );
  }

  getRelatoriosCards(params: any) {
    let httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString());

    if (params.status) httpParams = httpParams.set('status', params.status);
    if (params.search) httpParams = httpParams.set('search', params.search);
    if (params.searchField) httpParams = httpParams.set('searchField', params.searchField);
    if (params.searchJoin) httpParams = httpParams.set('searchJoin', params.searchJoin);
    if (params.include) httpParams = httpParams.set('include', params.include);

    return this.http.get<{ reports: any[]; totalItems: number }>(
      this.apiUrl + '/Relatorios',
      { headers: this.getHeaders(), params: httpParams }
    );
  }

  getReportCounts() {
    return this.http.get<ReportCounts>(this.apiUrl + '/Relatorio/KPIs', { headers: this.getHeaders() });
  }

  enviarSelecionados(ids: number[]): Observable<any> {
    return this.http.post(
      this.apiUrlIntegracaoBimmer + '/Inserir/Titulos/Manual',
      ids,
      { headers: this.getHeaders() }
    );
  }

  getErros(): Observable<{ items: any[]; totalCount: number }> {
    const url = this.apiUrlIntegracaoBimmer + '/Lista/Erros';

    return this.http.get<{ items: any[]; totalCount: number }>(url, {
      headers: this.getHeaders()
    });
  }

  getSucessos(params: GetRelatoriosParams): Observable<any> {

    const httpParams = new HttpParams()
      .set('pageNumber', params.pageNumber.toString())
      .set('pageSize', params.pageSize.toString())
      .set('status', params.status || '')
      .set('search', params.search || '');

    return this.http.get<any>(this.apiUrlIntegracaoBimmer + '/Lista/Erros', {
      headers: this.getHeaders(),
      params: httpParams
    });
  }
  //#endregion
}
