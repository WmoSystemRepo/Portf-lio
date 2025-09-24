
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '../core/services/anypoint/auth/auth.service';
import { SidebarComponent } from './sidebar/sidebar.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    SidebarComponent
  ],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {

  // #region Variáveis de estado

  isSidebarCollapsed = false;

  // #endregion

  // #region Construtor

  constructor(public authService: AuthService) { }

  // #endregion

  // #region Métodos públicos

  toggleSidebar(collapsed: boolean) {
    this.isSidebarCollapsed = collapsed;
  }

  // #endregion
}