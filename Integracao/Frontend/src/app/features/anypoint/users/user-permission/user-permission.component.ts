import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApplicationUser } from '../../../../shared/models/anypoint/application-user.model';
import { Menu } from '../../../../core/services/anypoint/menu/menu.service';
import { AuthService } from '../../../../core/services/anypoint/auth/auth.service';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-user-permission',
  standalone: true,
  templateUrl: './user-permission.component.html',
  styleUrls: ['./user-permission.component.css'],
  imports: [CommonModule],
})
export class UserPermissionComponent implements OnChanges {
  @Input() userId!: string;
  @Output() close = new EventEmitter<void>();

  user!: ApplicationUser;
  availableMenus: Menu[] = [];
  userPermissions: number[] = [];

  ngOnChanges(): void {
    if (this.userId) {
      // this.loadUser();
      // this.loadMenus();
      // this.loadUserPermissions();
    }
  }
  constructor(
    private route: ActivatedRoute,
    private userService: AuthService,
    private router: Router

  ) { }

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('id') || '';
    // this.loadUser();
    // this.loadMenus();
    // this.loadUserPermissions();
  }

  // loadUser(): void {
  //   this.userService.getUser(this.userId).subscribe(data => this.user = data);
  // }

  // loadMenus(): void {
  //   // Carregar os menus disponíveis (endpoints) da API
  //   this.userService.getMenus().subscribe(data => {
  //     this.availableMenus = data;
  //   });
  // }

  // loadUserPermissions(): void {
  //   this.userService.getUserPermissions(this.userId).subscribe((data: any[]) => {
  //     this.userPermissions = data.map(p => p.menuId);
  //   });
  // }

  togglePermission(menuId: number): void {
    const index = this.userPermissions.indexOf(menuId);
    if (index > -1) {
      // Remove a permissão
      this.userPermissions.splice(index, 1);
    } else {
      // Adiciona a permissão
      this.userPermissions.push(menuId);
    }
  }

  // savePermissions(): void {
  //   // Prepara o payload com as permissões
  //   const payload: UserEndpointPermission[] = this.userPermissions.map(menuId => ({
  //     id: 0, // o id pode ser ignorado ou tratado na API
  //     userId: this.userId,
  //     menuId: menuId
  //   }));
  //   this.userService.updateUserPermissions(this.userId, payload).subscribe(() => {
  //     this.router.navigate(['/users/listagem']);
  //   });
  // }

  closeModal() {
    this.close.emit();
  }
}