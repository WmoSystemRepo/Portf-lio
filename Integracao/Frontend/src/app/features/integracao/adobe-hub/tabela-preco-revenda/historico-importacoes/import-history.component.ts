import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ImportHistory } from '../../../../shared/models/anypoint/import-history.model';
import { ImportHistoryService } from '../../../../../Core/services/integracoes/adobe-hub/import-history.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-import-history',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MatButtonModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './import-history.component.html',
  styleUrls: ['./import-history.component.css']
})
export class ImportHistoryComponent implements OnInit {
  historyList: ImportHistory[] = [];
  filteredHistory: ImportHistory[] = [];
  paginatedHistory: ImportHistory[] = [];
  isLoading = false;
  errorMessage: string | null = null;

  activeFilter: string = 'all';
  daysFilter: string | number = 'all';
  searchTerm: string = '';
  currentPage: number = 1;
  itemsPerPage: number = 16;
totalPages: any;

  constructor(private importHistoryService: ImportHistoryService) {}

  ngOnInit(): void {
    const state = history.state;
    if (state?.filter === 'success') {
      this.activeFilter = 'success';
    } else if (state?.filter === 'failed') {
      this.activeFilter = 'failed';
    }

    this.loadHistory();
  }

  loadHistory(): void {
    this.isLoading = true;

    this.importHistoryService.getAll().subscribe({
      next: (data: ImportHistory[]) => {

        this.historyList = data.map(h => ({
          ...h,
          attemptDate: new Date(h.attemptDate),
          pendencias: h.pendencias || []
        }));

        this.applyFilter();
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Erro ao carregar histórico.';
        this.isLoading = false;
      }
    });
  }

  applyFilter(): void {
    let filtered = [...this.historyList];

    if (this.daysFilter !== 'all') {
      const days = Number(this.daysFilter);
      const now = new Date();
      const pastDate = new Date();
      pastDate.setDate(now.getDate() - days);
      filtered = filtered.filter(h => h.attemptDate >= pastDate);
    }

    if (this.activeFilter === 'success') {
      filtered = filtered.filter(h => h.success);
    } else if (this.activeFilter === 'failed') {
      filtered = filtered.filter(h => !h.success);
    }

    if (this.searchTerm.trim() !== '') {
      const term = this.searchTerm.trim().toLowerCase();
      filtered = filtered.filter(h =>
        h.templateName.toLowerCase().includes(term) ||
        h.fileName.toLowerCase().includes(term)
      );
    }

    filtered.sort((a, b) => b.attemptDate.getTime() - a.attemptDate.getTime());

    this.filteredHistory = filtered;
    this.currentPage = 1;
    this.updatePaginatedItems();
  }

  updatePaginatedItems(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedHistory = this.filteredHistory.slice(startIndex, endIndex);
  }

  goToPage(page: number): void {
    this.currentPage = page;
    this.updatePaginatedItems();
  }

  deleteHistory(id: string): void {
    if (confirm('Tem certeza que deseja excluir este histórico?')) {
      this.importHistoryService.deleteById(id).subscribe({
        next: () => {
          this.historyList = this.historyList.filter(h => h.id !== id);
          this.applyFilter();
        },
        error: (err: any): void => {
          this.errorMessage = 'Erro ao excluir item.';
        }
      });
    }
  }

  deleteAllHistory(): void {
    if (confirm('⚠ Tem certeza que deseja apagar TODOS os históricos? Esta ação não pode ser desfeita.')) {
      const idsToDelete = this.historyList.map(h => h.id);
      if (idsToDelete.length === 0) {
        return;
      }

      this.isLoading = true;

      const deleteRequests = idsToDelete.map(id => this.importHistoryService.deleteById(id));

      forkJoin(deleteRequests).subscribe({
        next: () => {
          this.afterBulkDelete();
        },
        error: (err) => {
          this.errorMessage = 'Erro ao apagar todos os históricos.';
          this.afterBulkDelete();
        }
      });
    }
  }

  afterBulkDelete(): void {
    this.loadHistory();
  }
}
