import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PendenciasVexpenseModel } from '../../../../shared/models/anypoint/pendencias-vexpense.model';

//#region Interface
export interface GetRelatoriosParams {
  pageNumber: number;
  pageSize: number;
  status?: string;
  search?: string;
  searchField?: string;
  searchJoin?: string;
  include?: string;
}
//#endregion

@Injectable({ providedIn: 'root' })
export class VExpensesService {
  private apiUrl = '/api/VExpenses';

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region PENDÊNCIAS
  getPendencias(params: GetRelatoriosParams): Observable<PendenciasVexpenseModel[]> {
    const queryParams: any = {
      pageNumber: params.pageNumber,
      pageSize: params.pageSize
    };

    if (params.search) {
      queryParams.search = params.search;
    }

    if (params.status) {
      queryParams.status = params.status;
    }

    return this.http.get<PendenciasVexpenseModel[]>(`${this.apiUrl}/ListarPendencias`, {
      params: queryParams
    });
  }

  buscarPendenciasIntegracao(pageNumber: number, pageSize: number, search?: string) {
    const params: any = {
      pageNumber,
      pageSize
    };

    if (search) {
      params.search = search;
    }

    return this.http.get<any>(`${this.apiUrl}/ListarPendencias`, { params });
  }
  //#endregion

  //#region ERROS
  buscarHistoricoErrosIntegracao(pageNumber: number, pageSize: number, search?: string) {
    const params: any = {
      pageNumber,
      pageSize
    };

    if (search) {
      params.search = search;
    }

    return this.http.get<any>(`${this.apiUrl}/ListarHistoricoErros`, { params });
  }
  //#endregion

  //#region RELATÓRIOS
  buscarRelatorioPorStatus(status: string, filtros: any) {
    return this.http.get(`${this.apiUrl}/Relatorio`, { params: { status, ...filtros } });
  }

  alteraStatusRelatorio(id: number, data: any) {
    return this.http.put(`${this.apiUrl}/alterarStatus?id=${id}`, data);
  }

  buscarUsuario(id: number) {
    return this.http.get(`${this.apiUrl}/buscarUsuario/${id}`);
  }

  listarRelatorios(params: {
    pageNumber?: number;
    pageSize?: number;
    status?: string;
    search?: string;
  }) {
    return this.http.get<any>(`${this.apiUrl}/Listar/Titulos`, { params });
  }

  obterContagensRelatorios() {
    return this.http.get(`${this.apiUrl}/ObterContagensRelatorios`);
  }
  //#endregion
}
