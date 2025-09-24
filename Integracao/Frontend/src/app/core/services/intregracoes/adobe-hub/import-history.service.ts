import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ImportHistory } from '../../../../shared/models/anypoint/import-history.model';

@Injectable({ providedIn: 'root' })
export class ImportHistoryService {
  private apiUrl = '/api/TemplatesPlanilha';

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region HISTÓRICO
  getAll(): Observable<ImportHistory[]> {
    return this.http.get<ImportHistory[]>(`${this.apiUrl}/listar-historico`);
  }

  deleteById(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/remover-historico/${id}`);
  }
  //#endregion
}