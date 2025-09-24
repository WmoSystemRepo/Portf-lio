import { Component, EventEmitter, Output, input } from '@angular/core';
import { AuthService } from '../../../../Core/services/auth/auth.service';
import { PermicoesService } from '../../../../Core/services/anypoint/permicoes.service';

@Component({
  standalone: true,
  selector: 'app-vincular-permissao-a-usuario',
  templateUrl: './vincular-permissao-a-usuario.component.html',
  styleUrl: './vincular-permissao-a-usuario.component.css'
})
export class VincularPermissaoAUsuarioComponent {
  readonly role = input<any>();
  readonly permission = input<any>();
  @Output() close = new EventEmitter();
  @Output() usersLinked = new EventEmitter();
  selectedUsers: string[] = [];
  users: any[] = [];
  linkedUsers: any[] = [];


  constructor(
    private userService: AuthService,
    private permissionService: PermicoesService
  ) { }

  ngOnInit() {
    this.loadUsers();
  }

  loadUsers() {
  }

  closeModal() {
    this.close.emit();
  }

  linkUsers() {
    const requestPayload = {
      userIds: this.selectedUsers,
      permissionId: this.permission().id
    };
  }

  onUserSelectionChange(event: Event, userId: string) {
    const checkbox = event.target as HTMLInputElement;

    if (checkbox.checked) {
      this.selectedUsers.push(userId);
    } else {
      const index = this.selectedUsers.indexOf(userId);
      if (index !== -1) {
        this.selectedUsers.splice(index, 1);
      }
    }
  }
}
