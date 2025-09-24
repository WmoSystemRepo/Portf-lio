import { DebugElement, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { PermicoesModel } from '../../../../shared/models/anypoint/permicoes.model';

@Injectable({
  providedIn: 'root'
})
export class PermicoesService {

  private apiUrl = `${environment.api}`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region PERMISSÕES 
  get(): Observable<PermicoesModel[]> {
    return this.http.get<PermicoesModel[]>(`${this.apiUrl}/Permicoes/Lista`);
  }

  getById(id: string): Observable<PermicoesModel> {
    return this.http.get<PermicoesModel>(`${this.apiUrl}/Permicoes/${id}`);
  }

  create(modelRetorno: PermicoesModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/Permicoes/Novo/`, modelRetorno);
  }

  update(id: any, model: PermicoesModel): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });

    return this.http.put(`${this.apiUrl}/Permicoes/Editar/${id}`, model, { headers });
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Permicoes/Excluir/${id}`);
  }
  //#endregion
}
