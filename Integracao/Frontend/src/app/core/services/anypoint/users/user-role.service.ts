import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserRoleService {
  private apiUrl = environment.api;

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

  //#region USUÁRIO REGRA 
  getUsersByRole(roleName: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/userroles/ObterUsuariosPorRegra/${roleName}`, { headers: this.getHeaders() });
  }

  assignUserToRole(userId: string, roleName: string): Observable<void> {
    return this.http.post<any>(`${this.apiUrl}/userroles/VincularUsuarioARegra`, { userId, roleName }, { headers: this.getHeaders() });
  }

  removeUserFromRole(userId: string, roleName: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/userroles/RemoverUsuarioARegra`, { userId, roleName }, { headers: this.getHeaders() });
  }
  //#endregion
}
