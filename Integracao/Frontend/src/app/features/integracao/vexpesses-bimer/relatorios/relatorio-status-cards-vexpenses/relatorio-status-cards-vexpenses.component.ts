import { CommonModule } from '@angular/common';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { PageEvent } from '@angular/material/paginator';

@Component({
    selector: 'app-relatorio-status-cards-vexpenses',
    imports: [
        CommonModule,
        MatCardModule,
        MatButtonModule
    ],
    templateUrl: './relatorio-status-cards-vexpenses.component.html',
    styleUrls: ['./relatorio-status-cards-vexpenses.component.css']
})
export class RelatorioStatusCardsVexpensesComponent {
selectedIds: any;
submitSelected() {
throw new Error('Method not implemented.');
}
applyFilter($event: Event) {
throw new Error('Method not implemented.');
}
dataSource: any;
onPageChange($event: PageEvent) {
throw new Error('Method not implemented.');
}
currentTabIndex: any;
totalItems: unknown;
pageSize: unknown;
pageNumber: any;
onTabChange($event: number) {
throw new Error('Method not implemented.');
}
iniciarTour() {
throw new Error('Method not implemented.');
}
  @Input() selectedFilter: string | null = null;

  @Input() totalGeral = 0;
  @Input() totalAbertos = 0;
  @Input() totalReaberto = 0;
  @Input() totalAprovados = 0;
  @Input() totalReprovado = 0;
  @Input() totalEnviado = 0;
  @Input() totalPago = 0;

  @Output() filterChange = new EventEmitter<string | null>();

  filterByCard(status: string | null): void {
    this.filterChange.emit(status);
  }
}
