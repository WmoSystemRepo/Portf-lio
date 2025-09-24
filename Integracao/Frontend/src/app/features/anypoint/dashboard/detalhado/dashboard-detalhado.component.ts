import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiLogAnyPoint } from '../../../../shared/models/anypoint/apiLogAnyPoint.model';
import { LogApiAnyPointService } from '../../../../core/services/anypoint/logs/log-api-any-point.service';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard-detalhado.component.html',
    styleUrls: ['./dashboard-detalhado.component.css'],
    imports: [CommonModule, FormsModule]
})
export class DashboardDetalhadoComponent implements OnInit {
  fields = [
    { name: 'nomeApi', label: 'API' },
    { name: 'grupo', label: 'Grupo' },
    { name: 'statusText', label: 'Status' },
    { name: 'tempoMs', label: 'Tempo (ms)' },
    { name: 'erro', label: 'Erro' }
  ];

  visibleFields: string[] = [];
  columnFilters: { [key: string]: string } = {};
  isDetailsModalOpen = false;
  isConfigModalOpen = false;

  itemToView: ApiLogAnyPoint | null = null;

  constructor(
    private http: HttpClient,
    public apiLogService: LogApiAnyPointService
  ) { }

  ngOnInit(): void {
    this.monitorarAnyPointStoreViaSwagger();

    const saved = localStorage.getItem('visibleFields_dashboard');
    if (saved) {
      try {
        const parsed = JSON.parse(saved);
        if (Array.isArray(parsed)) this.visibleFields = parsed;
      } catch {
      }
    }

    if (this.visibleFields.length === 0) {
      this.visibleFields = this.fields.map(f => f.name);
    }
  }

  async monitorarAnyPointStoreViaSwagger(): Promise<void> {
    const swaggerBase = environment.api.replace(/\/api$/, '');
    const swaggerUrl = `${swaggerBase}/swagger/v1/swagger.json`;

    try {
      const swagger = await firstValueFrom(this.http.get<any>(swaggerUrl));
      const paths = swagger.paths;

      for (const [path, methods] of Object.entries(paths)) {
        for (const [verb, operation] of Object.entries(methods as any)) {
          const op = operation as { tags?: string[] };
          const tags: string[] = op.tags || [];

          if (tags.some(tag => tag.startsWith('Sistema Any Point Store'))) {
            const fullUrl = `${environment.api}${path}`;
            this.testarEndpoint(fullUrl, verb.toUpperCase(), tags[0]);
          }
        }
      }
    } catch (err) {
    }
  }

  async testarEndpoint(url: string, metodo: string, grupo: string): Promise<void> {
    const inicio = performance.now();

    try {
      await firstValueFrom(this.http.request(metodo, url));
      const fim = performance.now();

      const log: ApiLogAnyPoint = {
        url,
        nomeApi: url,
        grupo,
        status: 200,
        statusText: 'OK',
        tempoMs: Math.round(fim - inicio),
        sucesso: true
      };

      this.apiLogService.registrarLog(log);
    } catch (err: any) {
      const fim = performance.now();

      const log: ApiLogAnyPoint = {
        url,
        nomeApi: url,
        grupo,
        status: err.status || 0,
        statusText: err.statusText || 'Erro',
        tempoMs: Math.round(fim - inicio),
        sucesso: false,
        erro: err.message || 'Erro desconhecido'
      };

      this.apiLogService.registrarLog(log);
    }
  }

  get filteredLogs(): ApiLogAnyPoint[] {
    return this.apiLogService.obterLogsRecentes().filter((item: { [x: string]: any; }) =>
      this.fields.every(field => {
        const filtro = this.columnFilters[field.name]?.toLowerCase().trim();
        if (!filtro) return true;
        const valor = String(item[field.name as keyof ApiLogAnyPoint] ?? '').toLowerCase();
        return valor.includes(filtro);
      })
    );
  }

  isFieldVisible(name: string): boolean {
    return this.visibleFields.includes(name);
  }

  isDateField(name: string): boolean {
    return name.toLowerCase().includes('data');
  }

  getFieldValue(item: ApiLogAnyPoint, fieldName: string): any {
    return item[fieldName as keyof ApiLogAnyPoint];
  }

  clearFilters(): void {
    this.columnFilters = {};
  }

  openDetailsModal(item: ApiLogAnyPoint): void {
    this.itemToView = item;
    this.isDetailsModalOpen = true;
  }

  closeDetailsModal(): void {
    this.itemToView = null;
    this.isDetailsModalOpen = false;
  }

  openGridConfig(): void {
    if (!this.visibleFields.length && this.fields.length) {
      this.visibleFields = this.fields.map(f => f.name);
    }
    this.isConfigModalOpen = true;
  }

  closeGridConfig(): void {
    this.isConfigModalOpen = false;
    localStorage.setItem('visibleFields_dashboard', JSON.stringify(this.visibleFields));
  }

  toggleFieldVisibility(fieldName: string): void {
    const index = this.visibleFields.indexOf(fieldName);
    if (index >= 0) {
      this.visibleFields.splice(index, 1);
    } else {
      this.visibleFields.push(fieldName);
    }

    localStorage.setItem('visibleFields_dashboard', JSON.stringify(this.visibleFields));
  }
}
