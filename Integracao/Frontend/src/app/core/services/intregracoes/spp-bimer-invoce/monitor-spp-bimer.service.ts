import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { SppBimerInvoceMonitorModel } from '../../../../shared/models/integracao/SppBimerInvoce/spp-bimerInvoce-monitor.model';
import { environment } from '../../../../../environments/environment';
import { ReprocessarBimerRequest } from '../../../../shared/models/integracao/SppBimerInvoce/ReprocessarBimerRequest.model';

@Injectable({ providedIn: 'root' })
export class MonitorSppBimerService {

  private http = inject(HttpClient);
  private apiUrl = `${environment.api}/Monitoramento/Spp/bimer/Invoce`;

  //#region MONITORAMENTO DE INVOICE

  getMonitoramentos(
    status?: string,
    dataInicio?: string,
    dataFim?: string
  ): Observable<SppBimerInvoceMonitorModel[]> {
    let params = new HttpParams();
    if (status) params = params.set('status', status);
    if (dataInicio) params = params.set('dataInicio', dataInicio);
    if (dataFim) params = params.set('dataFim', dataFim);

    return this.http.get<SppBimerInvoceMonitorModel[]>(this.apiUrl, { params });
  }

  listarTodos(): Observable<SppBimerInvoceMonitorModel[]> {
    return this.http.get<SppBimerInvoceMonitorModel[]>(`${this.apiUrl}/Lista`);
  }

  /**
   * Executa o reprocessamento usando o corpo da requisição, mas força o responseType como 'text'
   * para evitar [object Object] em respostas de sucesso.
   */
  reprocessar(request: ReprocessarBimerRequest): Observable<string> {
    return this.http.post(`${this.apiUrl}/Reprocessar`, request, {
      responseType: 'text'
    });
  }

  /**
   * Gera a URL completa usada para reprocessar a requisição (exibida no modal)
   */
  getReprocessamentoUrl(request: ReprocessarBimerRequest): string {
    return `${this.apiUrl}/Reprocessar?pedido=${request.pedido}&fabricante=${request.fabricante}&estoque=${request.estoque}`;
  }

  //#endregion
}
