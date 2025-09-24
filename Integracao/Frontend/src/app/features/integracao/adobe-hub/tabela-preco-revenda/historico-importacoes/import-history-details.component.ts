import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ImportHistory } from '../../../../shared/models/anypoint/import-history.model';
import { ImportHistoryService } from '../../../../../Core/services/integracoes/adobe-hub/import-history.service';

@Component({
  standalone: true,
  selector: 'app-import-history-details',
  templateUrl: './import-history-details.component.html',
  imports: [
    CommonModule
  ],
})
export class ImportHistoryDetailsComponent implements OnInit {
  isLoading = false;
  errorMessage = '';
  history: ImportHistory | null = null;

  constructor(
    private route: ActivatedRoute,
    private importHistoryService: ImportHistoryService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isLoading = true;

      this.importHistoryService.getAll().subscribe({
        next: (data) => {
          const item = data.find(h => h.id === id);
          if (item) {
            this.history = {
              ...item,
              attemptDate: new Date(item.attemptDate),
              pendencias: item.pendencias || []
            };
          } else {
            this.errorMessage = 'Histórico não encontrado.';
          }
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Erro ao buscar histórico.';
          this.isLoading = false;
        }
      });
    } else {
      this.errorMessage = 'ID inválido.';
    }
  }
}
