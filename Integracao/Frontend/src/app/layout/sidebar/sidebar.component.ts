import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { CommonModule } from '@angular/common';
import { AuthService, MenuService } from '../../features/anypoint/auth';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']

})
export class SidebarComponent implements OnInit {
  user: any = null;
  isCollapsed = false;
  menus: any[] = [];
  parentMenus: any[] = [];

  @Output() toggle = new EventEmitter<boolean>();

  constructor(
    private router: Router,
    private authService: AuthService,
    private menuService: MenuService
  ) { }

  ngOnInit(): void {
  
    this.user = this.authService.getUserInfo();

    this.menuService.ObterListaMenus().subscribe((data: any[]) => {
      this.menus = this.filtrarMenusPorPermissao(data);
      this.montarMenusComSubmenus();
    });

    const storedState = localStorage.getItem('sidebar-collapsed');
    this.isCollapsed = storedState === 'true';
  }

  private filtrarMenusPorPermissao(menus: any[]): any[] {
  
    let permitidos: number[] = [];

    try {
      const parsedMenus = JSON.parse(this.user?.Menus || '[]');
      permitidos = parsedMenus.map((item: any) => item.MenuId).filter((id: any) => typeof id === 'number');
    } catch (error) {
      console.warn('Erro ao processar os menus do usuário:', error);
    }

    return menus.filter(menu => permitidos.includes(menu.id));
  }

  toggleSidebar(): void {
    this.isCollapsed = !this.isCollapsed;
    localStorage.setItem('sidebar-collapsed', String(this.isCollapsed));
    this.toggle.emit(this.isCollapsed);
  }

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  handleMenuClick(event: Event, menu: any): void {
    event.stopPropagation();

    // Expande ou contrai o menu
    if (menu.subMenus && menu.subMenus.length > 0) {
      menu.isExpanded = !menu.isExpanded;
    }

    if (menu.rota) {
      this.navigateTo(menu.rota);
    }
  }

  montarMenusComSubmenus(): void {
    this.parentMenus = this.menus.filter(menu => menu.ehMenuPrincipal === true);

    this.parentMenus.forEach(menuPai => {
      menuPai.subMenus = this.menus.filter(sub =>
        sub.subMenuReferenciaPrincipal === menuPai.id &&
        sub.ehMenuPrincipal === false
      );
      menuPai.isExpanded = false;
    });

    this.parentMenus = this.parentMenus.filter(menuPai =>
      menuPai.subMenus.length > 0 || !!menuPai.rota
    );
  }

  canViewConfiguration(): boolean {
    const token = localStorage.getItem('auth');
    if (!token) return false;

    try {
      const decodedToken: any = jwtDecode(token);
      return decodedToken.PodeVerConfiguracao === 'True';
    } catch {
      return false;
    }
  }
}
