import { CommonModule } from '@angular/common';
import { Component, OnInit, input, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../../Core/services/auth.service';
import { PermissionService } from '../../../../Core/services/permission.service';

@Component({
  standalone: true,
  selector: 'app-vincular-usuario-permissao',
  templateUrl: './vincular-usuario-a-role.component.html',
  styleUrls: ['./vincular-usuario-a-role.component.ts'],
  imports: [
    CommonModule,
    FormsModule
  ]
})
export class VincularUsuarioPermissaoComponent implements OnInit {

  //#region Propriedades
  readonly permission = input<any>();
  @Output() close = new EventEmitter<void>();
  @Output() rolesLinked = new EventEmitter<any>();

  selectedRoles: number[] = [];
  roles: any[] = [];
  linkedRoles: any[] = [];
  //#endregion

  //#region Construtor
  constructor(
    private authService: AuthService,
    private permissionService: PermissionService
  ) { }
  //#endregion

  //#region Ciclo de vida
  ngOnInit(): void {
    this.loadRoles();
    this.loadLinkedRoles();
  }
  //#endregion

  //#region Ações
  loadRoles() {
    this.authService.getRoles().subscribe((roles: any[]) => {
      this.roles = roles;
    });
  }

  loadLinkedRoles() {
    this.permissionService.getRolesLinkedOnPermission(this.permission().id).subscribe((roles: any[]) => {
      this.linkedRoles = roles;
    });
  }

  linkRoles() {
    this.rolesLinked.emit(this.selectedRoles);
    this.fechar();
  }

  removeRole(roleId: string) {
    this.permissionService.removeRoleFromPermission(roleId, this.permission().id).subscribe(() => {
      this.loadLinkedRoles();
    });
  }

  fechar() {
    this.close.emit();
  }
  //#endregion
}