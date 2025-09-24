import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root' 
})

export class AclService {
  private userPermissions: string[] = [];
  private userRoles: string[] = [];

  constructor() {}

  setPermissions(permissions: string[]): void {
    this.userPermissions = permissions || [];
  }

  setRoles(roles: string[]): void {
    this.userRoles = roles || [];
  }

  can(permission: string): boolean {
    return this.userPermissions.includes(permission);
  }

  hasRole(role: string): boolean {
    return this.userRoles.includes(role);
  }

  clear(): void {
    this.userPermissions = [];
    this.userRoles = [];
  }

  getPermissions(): string[] {
    return this.userPermissions;
  }

  getRoles(): string[] {
    return this.userRoles;
  }
}
