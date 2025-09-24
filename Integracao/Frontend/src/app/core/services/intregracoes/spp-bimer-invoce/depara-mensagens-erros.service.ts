import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SppBimerInvoceDeparamensagem } from '../../../../shared/models/integracao/SppBimerInvoce/spp-bimerInvoce-deparamensagem.model';

@Injectable({
  providedIn: 'root'
})
export class MonitorDeparaMensagensService {
  
  private readonly apiUrl = '/api/DeParaMensagem';
  
  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region CRUD DE/PARA MENSAGEM
  listar(): Observable<SppBimerInvoceDeparamensagem[]> {
    return this.http.get<SppBimerInvoceDeparamensagem[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<SppBimerInvoceDeparamensagem> {
    return this.http.get<SppBimerInvoceDeparamensagem>(`${this.apiUrl}/${id}`);
  }

  buscarPorMensagemPadrao(mensagemPadrao: string): Observable<SppBimerInvoceDeparamensagem> {
    const encoded = encodeURIComponent(mensagemPadrao);
    return this.http.get<SppBimerInvoceDeparamensagem>(`${this.apiUrl}/${encoded}`);
  }

  criar(dto: SppBimerInvoceDeparamensagem): Observable<SppBimerInvoceDeparamensagem> {
    return this.http.post<SppBimerInvoceDeparamensagem>(this.apiUrl, dto);
  }

  atualizar(id: number, dto: SppBimerInvoceDeparamensagem): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, dto);
  }

  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  //#endregion
}