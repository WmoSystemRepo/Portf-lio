import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { TemplateModel } from '../../../../shared/models/integracao/AdobeHub/templante.model';


@Injectable({ providedIn: 'root' })
export class TemplateService {

  private apiUrl = `${environment.api}/Adobe/Planilhas`;


  constructor(private http: HttpClient) { }

  ObterListaTemplante(): Observable<TemplateModel[]> {
    debugger
    return this.http.get<TemplateModel[]>(`${this.apiUrl}/Lista/Templantes`);
  }

  NovoTemplante(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Novo/Templantes`, data);
  }


  updateTemplate(templateId: string, data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/atualizar/${templateId}`, data);
  }

  getTemplateById(templateId: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/buscar-por-id/${templateId}`);
  }

  ExcluirTemplate(payload: any): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Excluir/Template`, 
    {
      //headers: this.getHeaders(),
      body: payload
    });
  }

  buscarPorNomeTemplate(nomeTemplate: string): Observable<TemplateModel> {
    return this.http.get<TemplateModel>(`${this.apiUrl}/buscar/${nomeTemplate}`);
  }

  importarDocumento(data: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/importarDocumento`, data);
  }

  validarDocumento(data: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/validarDocumento`, data);
  }
  //#endregion
}
