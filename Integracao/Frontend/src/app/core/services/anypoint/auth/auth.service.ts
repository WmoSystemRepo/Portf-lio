import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { ApplicationUser } from '../../../../shared/models/anypoint/application-user.model';
import { jwtDecode, JwtPayload } from 'jwt-decode';
import { UserEndpointPermission } from '../../../../shared/models/anypoint/user-endpoint-permission.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  getUser: any;
  updateUserViaMyProfile: any;
  updateUserPermissions(userId: string, payload: UserEndpointPermission[]) {
    throw new Error('Method not implemented.');
  }

  getRoles(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/users/GetRoles`, { headers: this.getHeaders() });
  }

  private apiUrl = environment.api;
  logoutTimer: any;

  constructor(private http: HttpClient) { }

  private getHeaders(): HttpHeaders {
    const token = this.getToken();
    return new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: `Bearer ${token}`
    });
  }

  login(payload: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(`${environment.api}/account/login`, payload, { headers }).pipe(
      tap(
        (response: any) => {
          localStorage.setItem('auth', response.token);
          localStorage.setItem('refresh_token', response.refreshToken);
        },
        error => {
        }
      )
    );
  }

  getUserProfile(): Observable<ApplicationUser> {
    return this.http.get<ApplicationUser>(`${environment.api}/account/profile`);
  }

  changesenha(newsenha: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/auth/change-password`, newsenha, { headers: this.getHeaders() });
  }

  forgotsenha(email: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/auth/forgot-password`, { email });
  }

  logout(): void {
    localStorage.removeItem('auth');
    localStorage.removeItem('refresh_token');
  }

  getToken(): string | null {
    return localStorage.getItem('auth');
  }

  getRefreshToken(): string | null {
    return localStorage.getItem('refresh_token');
  }

  isTokenExpired(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      if (decoded.exp === undefined) return true;
      const expirationTime = decoded.exp * 1000;
      return Date.now() >= expirationTime;
    } catch (error) {
      return true;
    }
  }

  firstLogin(): boolean {
    const token = this.getToken();
    if (!token) return true;

    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.FirstLogin === 'True';
    } catch (error) {
      return false;
    }
  }

  getUserInfo(): any {
    const token = this.getToken();

    if (!token) {
      return null;
    }

    try {
      const decoded = jwtDecode(token);
      return decoded;
    } catch (error) {
      return null;
    }
  }

  startAutoLogout(): void {
    const token = this.getToken();
    if (!token) return;

    try {
      const decoded = jwtDecode<any>(token);
      if (decoded.exp === undefined) return;

      const expirationTime = decoded.exp * 1000;
      const now = Date.now();
      const timeout = expirationTime - now;

      if (timeout <= 0) {
        this.performAutoLogout();
        return;
      }

      if (this.logoutTimer) {
        clearTimeout(this.logoutTimer);
      }

      this.logoutTimer = setTimeout(() => {
        this.performAutoLogout();
      }, timeout);

    } catch (error) {
    }
  }

  performAutoLogout(): void {
    this.logout();
    window.location.href = '/auth';
  }

  getDecodedToken(): any {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode(token);
    } catch (error) {
      return null;
    }
  }
}
