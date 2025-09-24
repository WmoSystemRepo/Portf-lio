import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { Cotacao, CotacaoResponse } from '../../../../shared/models/integracao/Bacen/cotacao.model';

@Injectable({
  providedIn: 'root',
})
export class CambioService {
  private readonly apiUrl = `${environment.api}/cambio`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region COTAÇÃO DOLAR 
  getCotacoes(): Observable<CotacaoResponse> {
    return this.http.get<CotacaoResponse>(`${this.apiUrl}/listagem`);
  }

  getCotacaoDolar(dataInicio: string, dataFim: string, codigoMoeda: string): Observable<Cotacao[]> {
    return this.http.get<Cotacao[]>(`${this.apiUrl}/cotacao-dolar`, {
      params: { dataInicio, dataFim, codigoMoeda },
    });
  }
  //#endregion
}
