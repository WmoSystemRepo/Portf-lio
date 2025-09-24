import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApplicationUser } from '../../../../shared/models/anypoint/application-user.model';
import { RegisterUser } from '../../../../shared/models/anypoint/RegisterUser.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
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

  //#region USUÁRIO
  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/Usuario/Lista`, { headers: this.getHeaders() });
  }

  getUser(id: string): Observable<ApplicationUser> {
    return this.http.get<ApplicationUser>(`${this.apiUrl}/Usuario/${id}`, { headers: this.getHeaders() });
  }

  createUser(user: RegisterUser): Observable<{ id: string; userName: string }> {
    return this.http.post<{ id: string; userName: string }>(`${this.apiUrl}/Usuario/Novo`, user, { headers: this.getHeaders() });
  }

  updateUser(user: ApplicationUser): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuario/Editar/${user.id}`, user, { headers: this.getHeaders() });
  }

  updateUserViaMyProfile(user: ApplicationUser): Observable<any> {
    return this.http.put(`${this.apiUrl}/Usuario/updateUserViaMyProfile/${user.id}`, user, { headers: this.getHeaders() });
  }

  deleteUser(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Usuario/${id}`, { headers: this.getHeaders() });
  }
  //#endregion
}
