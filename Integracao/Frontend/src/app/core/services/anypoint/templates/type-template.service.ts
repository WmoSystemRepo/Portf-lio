import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TipoTemplate } from '../../../../shared/models/anypoint/tipo-template.model';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import { environment } from '../../../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class TypeTemplateService {
  private readonly apiUrl = '/api/Tipo/Templante';

    //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region TIPO TEMPLATE
  listar(): Observable<TipoTemplate[]> {
    return this.http.get<TipoTemplate[]>(this.apiUrl);
  }

  buscarPorId(id: number): Observable<TipoTemplate> {
    return this.http.get<TipoTemplate>(`${this.apiUrl}/${id}`);
  }

  criar(dto: TipoTemplate): Observable<TipoTemplate> {
    return this.http.post<TipoTemplate>(this.apiUrl, dto);
  }

  atualizar(id: number, dto: TipoTemplate): Observable<TipoTemplate> {
    return this.http.put<TipoTemplate>(`${this.apiUrl}/${id}`, dto);
  }

  excluir(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
  //#endregion

  //#region INTEGRAÇÕES
  ObterListaGestaoIntegracoes(): Observable<GestaoIntegracaoModel[]> {
    return this.http.get<GestaoIntegracaoModel[]>(`${environment.api}/GestaoIntegracoes/Lista`);
  }
  //#endregion
}