import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ListaCrudComponent } from '../../../../shared/components/anypoint/lista-crud/lista-crud.component';
import { TipoTemplate } from '../../../../shared/models/anypoint/tipo-template.model';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';
import { TypeTemplateService } from '../../../../core/services/anypoint/templates/type-template.service';
import Shepherd from 'shepherd.js';

@Component({
  selector: 'app-type-template-list',
  standalone: true,
  imports: [CommonModule, ListaCrudComponent],
  templateUrl: './type-template-list.component.html',
})
export class TypeTemplateListComponent implements OnInit {

  //#region Propriedades
  model: TipoTemplate[] = [];

  displayFields:
    | [
      { name: 'nomeCompleto'; label: 'Nome Completo'; align: 'left' },
      { name: 'sigla'; label: 'Sigla'; align: 'center' },
      { name: 'integracaoId'; label: 'Integração'; align: 'right' }
    ]
    | undefined;

  tour = new Shepherd.Tour({
    defaultStepOptions: {
      scrollTo: true,
      cancelIcon: { enabled: true },
      classes: 'shepherd-theme-arrows'
    },
    useModalOverlay: true
  });

  //#endregion

  //#region Construtor
  constructor(
    private service: TypeTemplateService,
    private router: Router
  ) { }
  //#endregion

  //#region Ciclo de vida
  ngOnInit(): void {
    this.load();
  }
  //#endregion

  //#region Ações CRUD
  load(): void {
    this.service.listar().subscribe({
      next: (data: TipoTemplate[]) => {
        this.model = data;
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao carregar os TipoTemplates.', 'error');
      }
    });
  }

  add(): void {
    this.router.navigate(['/Tipos/Templates/Novo']);
  }

  edit(item: TipoTemplate): void {
    this.router.navigate(['/Tipos/Templates/Editar/', item.id?.toString()]);
  }

  confirmDelete(item: TipoTemplate): void {
    if (!item.id) return;
    this.service.excluir(item.id).subscribe({
      next: () => {
        this.load();
        Swal.fire('Sucesso', 'TipoTemplate deletado com sucesso.', 'success');
      },
      error: () => {
        Swal.fire('Erro', 'Erro ao deletar o TipoTemplate.', 'error');
      }
    });
  }

  carregarDadosNovamente(): void {
    window.location.reload();
  }
  //#endregion
}