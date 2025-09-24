import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { PlanilhasImportadasModel } from '../../../../shared/models/integracao/AdobeHub/planilhas-importadas.model';

@Injectable({ providedIn: 'root' })
export class PlaninhaImportadasService {
  private apiUrl = `${environment.api}/Adobe/Planilhas`;

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

  //#region PLANILHA 
  ObterPlaninhas(): Observable<PlanilhasImportadasModel[]> {
    return this.http.get<PlanilhasImportadasModel[]>(
      `${this.apiUrl}/Lista/Planilhas/Importadas`
    );
  }
  
  SalvarNovaPlanilhaImportada(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Importar/Novo/Excel`, data);
  }

  updateTemplate(templateId: string, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/atualizar/${templateId}`, data);
  }

  getTemplateById(templateId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/buscar-por-id/${templateId}`);
  }

  ExcluirPLanilhaImportada(payload: any): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Excluir/Planilha`, {
      body: payload
    });
  }

  buscarPorNomeTemplate(
    nomeTemplate: string
  ): Observable<PlanilhasImportadasModel> {
    return this.http.get<PlanilhasImportadasModel>(
      `${this.apiUrl}/buscar/${nomeTemplate}`
    );
  }

  importarDocumento(data: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/importarDocumento`, data);
  }

  validarDocumento(data: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/validarDocumento`, data);
  }

  ObterPlanilhaPorId(id: string): Observable<PlanilhasImportadasModel> {
    return this.http.get<PlanilhasImportadasModel>(
      `${this.apiUrl}/Obter/Id/${id}`
    );
  }

  AdicionarProdutoPlaninha(planilhaId: string, produto: any): Observable<{ status: string }> {
    return this.http.post<{ status: string }>(
      `${this.apiUrl}/Planilha/${planilhaId}/Produtos/Salvar`,
      { produto },
      { headers: this.getHeaders() }
    );
  }
  //#endregion
}