import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { UnauthorizedComponent } from '../../../shared/components/anypoint/unauthorized/unauthorized.component';

@Component({
  standalone: true,
  selector: 'app-configuration',
  templateUrl: './configuracao.componente.html',
  styleUrls: ['./configuracao.componente.scss'],
  imports: [CommonModule]
})
export class ConfigurationComponent {
  isSidebarCollapsed = false;
  unauthorizedComponent = UnauthorizedComponent;

  constructor(private router: Router) { }

  toggleSidebar(collapsed: boolean) {
    this.isSidebarCollapsed = collapsed;
  }

  navigateTo(route: string) {
    this.router.navigate([route]);
  }

  canViewConfiguration(): boolean {
    const token = localStorage.getItem('auth');
    if (!token) {
      return false;
    }

    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.PodeVerConfiguracao === 'True';
    } catch (error) {
      return false;
    }
  }
}
