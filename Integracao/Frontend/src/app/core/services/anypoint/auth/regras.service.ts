import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { RegraModel } from '../../../../shared/models/anypoint/regra.model';

@Injectable({
  providedIn: 'root'
})
export class RegraService {

  private apiUrl = `${environment.api}`;

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region REGRAS
  get(): Observable<RegraModel[]> {
    return this.http.get<RegraModel[]>(`${this.apiUrl}/Regra/Lista`);
  }

  getById(id: string): Observable<RegraModel> {
    return this.http.get<RegraModel>(`${this.apiUrl}/Regra/${id}`);
  }

  create(modelRetorno: RegraModel): Observable<any> {
    return this.http.post(`${this.apiUrl}/Regra/Nova/`, modelRetorno);
  }

  update(id: string, model: RegraModel): Observable<any> {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json'
    });
    return this.http.put(`${this.apiUrl}/Regra/Editar/${id}`, model, { headers });
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Regra/Excluir/${id}`);
  }
  //#endregion
}