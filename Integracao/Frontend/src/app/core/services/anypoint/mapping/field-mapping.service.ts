import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GestaoMapearCamposModel } from '../../../../shared/models/anypoint/gestao-mapear-campos.model';
import { GestaoIntegracaoModel } from '../../../../shared/models/anypoint/gestao-integracao.model';
import { environment } from '../../../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class FieldMappingService {

  private apiUrl = `${environment.api}/GestaoIntegracoes`;

  //#region Constructor
  constructor(private http: HttpClient) { }
  //#endregion

  //#region MAPEAR CAMPOS
  ObterListaGestaoMapearCampos(): Observable<GestaoMapearCamposModel[]> {
    const url = `${this.apiUrl}/Lista`;
    console.log('📡 Chamando URL:', url); // ✅ Log de inspeção
    return this.http.get<GestaoMapearCamposModel[]>(url);
  }


  ObterGestaoMapearCampos(integracao: string): Observable<GestaoMapearCamposModel[]> {
    return this.http.get<GestaoMapearCamposModel[]>(`${this.apiUrl}/Lista/${integracao}`);
  }

  ObterGestaoMapearCamposPorId(id: string): Observable<GestaoMapearCamposModel> {
    return this.http.get<GestaoMapearCamposModel>(`${this.apiUrl}/Id/${id}`);
  }

  createMapeamento(data: GestaoMapearCamposModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/Novo`, data);
  }

  updateMapeamento(id: string, data: GestaoMapearCamposModel): Observable<any> {
    return this.http.put(`${this.apiUrl}/Editar/${id}`, data);
  }

  deleteMapeamento(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/Excluir/${id}`);
  }
  //#endregion

  //#region INTEGRAÇÕES
  ObterListaGestaoIntegracoes(): Observable<GestaoIntegracaoModel[]> {
    return this.http.get<GestaoIntegracaoModel[]>(
      `${environment.api}/GestaoIntegracoes/Lista`
    );
  }

  RegistrarDeparaIntegracaoReferencia(payload: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/Integracao/Novo`, payload);
  }

  ObterDeparaIntegracaoPorIdReferencia(id: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/Integracao/IdReferencia/${id}`);
  }

  ObterMapeamentoIntegracoesReferenciaPorMapeamentoId(mapeamentoId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Integracao/IdMapeamento/${mapeamentoId}`);
  }

  ExcluirIntegracaoReferencia(MapeamentoId: string, IntegracaoId: string): Observable<any> {
    return this.http.delete(
      `${this.apiUrl}/Integracao/Excluir/Referencia/${MapeamentoId}/${IntegracaoId}`
    );
  }
  //#endregion
}