import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { environment } from '../../../../environments/environment';
import { Route, Router } from '@angular/router';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.css'],
    imports: [NgChartsModule, CommonModule]
})
export class DashboardComponent implements OnInit {

  modoExpandido = false;

  totalUsuarios = 0;
  totalIntegracoes = 0;
  totalPermissoes = 0;
  totalRoles = 0;
  totalMenus = 0;
  totalMapeamentos = 0;
  totalDePara = 0;

  barChartUsersType: ChartType = 'bar';
  barChartUsersOptions: ChartConfiguration['options'] = { responsive: true };
  barChartUsersData: ChartData<'bar'> = { labels: [], datasets: [] };

  lineChartType: ChartType = 'line';
  lineChartOptions: ChartConfiguration['options'] = { responsive: true };
  lineChartData: ChartData<'line'> = { labels: [], datasets: [] };

  permissoesMenuChartType: ChartType = 'bar';
  permissoesMenuChartOptions: ChartConfiguration['options'] = { responsive: true };
  permissoesMenuChartData: ChartData<'bar'> = { labels: [], datasets: [] };

  pieChartDeParaData: ChartData<'pie'> = { labels: [], datasets: [] };
  pieChartIntegracoesData: ChartData<'pie'> = { labels: [], datasets: [] };
  pieChartIntegracoesDetalhadasData: ChartData<'pie'> = { labels: [], datasets: [] };
  pieChartAnyPointStoreData: ChartData<'pie'> = { labels: [], datasets: [] };

  get pieChartOptions(): ChartConfiguration<'pie'>['options'] {
    return {
      responsive: true,
      plugins: {
        legend: {
          position: this.modoExpandido ? 'right' : 'bottom',
          align: this.modoExpandido ? 'start' : 'center',
          labels: {
            color: '#333',
            font: { size: 12 },
            boxWidth: 16,
            padding: 8,
            textAlign: 'left',
            usePointStyle: true
          }
        },
        datalabels: {
          formatter: (value, context) => {
            const data = (context.chart.data.datasets[0].data as number[])
              .filter((d): d is number => typeof d === 'number');
            const total = data.reduce((acc, val) => acc + val, 0);
            return `${((value as number / total) * 100).toFixed(1)}%`;
          },
          color: '#fff',
          font: { weight: 'bold', size: 14 }
        }
      }
    };
  }

  constructor(
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    const salvo = localStorage.getItem('modoExpandido');
    this.modoExpandido = salvo === 'true';

    this.loadKPIs();
    this.loadUsersByRole();
    this.loadIntegracoesPorMes();
    this.loadPermissoesPorMenu();
    this.loadApisPorGrupoViaSwagger();
    this.loadResumoDeGruposSwagger();
    this.loadDetalhesDasIntegracoesViaSwagger();
    this.loadApisAnyPointStore();
  }

  loadKPIs(): void {
    this.http.get<any[]>(`${environment.api}/Usuario/Lista`).subscribe(x => this.totalUsuarios = x.length);
    this.http.get<any[]>(`${environment.api}/GestaoIntegracoes/Lista`).subscribe(x => this.totalIntegracoes = x.length);
    this.http.get<any[]>(`${environment.api}/Permicoes/Lista`).subscribe(x => this.totalPermissoes = x.length);
    this.http.get<any[]>(`${environment.api}/Regra/Lista`).subscribe(x => this.totalRoles = x.length);
    this.http.get<any[]>(`${environment.api}/Menu/Lista`).subscribe(x => this.totalMenus = x.length);
    this.http.get<any[]>(`${environment.api}/GestaoMapearCampos/Lista`).subscribe(x => this.totalMapeamentos = x.length);
  }

  async loadUsersByRole(): Promise<void> {
    const roles = await firstValueFrom(this.http.get<{ nome: string }[]>(`${environment.api}/Regra/Lista`));
    const labels: string[] = [];
    const values: number[] = [];

    for (const role of roles) {
      const users = await firstValueFrom(this.http.get<any[]>(`${environment.api}/user-roles/obter-usuarios-por-regra/${role.nome}`));
      labels.push(role.nome);
      values.push(users.length);
    }

    this.barChartUsersData = {
      labels,
      datasets: [{ data: values, label: 'Usuários por Role' }]
    };
  }

  async loadIntegracoesPorMes(): Promise<void> {
    const data = await firstValueFrom(this.http.get<any[]>(`${environment.api}/GestaoIntegracoes/Lista`));
    const meses = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    const contagem = Array(12).fill(0);

    for (const item of data) {
      if (item.dataCriacao) contagem[new Date(item.dataCriacao).getMonth()]++;
    }

    this.lineChartData = {
      labels: meses,
      datasets: [{ data: contagem, label: 'Integrações por Mês' }]
    };
  }

  async loadPermissoesPorMenu(): Promise<void> {
    const permissoes = await firstValueFrom(this.http.get<any[]>(`${environment.api}/Permicoes/Lista`));
    const labels = permissoes.map(p => p.nome);
    const data = permissoes.map(p => p.permissoesDoMenu || 0);

    this.permissoesMenuChartData = {
      labels,
      datasets: [{ data, label: 'Permissões por Menu' }]
    };
  }

  async loadApisPorGrupoViaSwagger(): Promise<void> {
    const swagger = await this.getSwagger();
    const contagem: Record<string, number> = {};

    for (const methods of Object.values(swagger.paths)) {
      for (const op of Object.values(methods as any)) {
        const operation = op as { tags?: string[] };
        const tags: string[] = Array.isArray(operation.tags) ? operation.tags : [];

        for (const tag of tags) {
          if (tag.startsWith('Sistema Any Point Store') || /^\d+[\.: -]/.test(tag)) continue;
          contagem[tag] = (contagem[tag] || 0) + 1;
        }
      }
    }


    const entries = Object.entries(contagem).sort((a, b) => b[1] - a[1]);
    this.pieChartIntegracoesData = {
      labels: entries.map(e => e[0]),
      datasets: [{ data: entries.map(e => e[1]), backgroundColor: this.getPieColors(entries.length) }]
    };
  }

  async loadResumoDeGruposSwagger(): Promise<void> {
    const swagger = await this.getSwagger();
    const contagem = { 'Any Point Store': 0, 'Integrações': 0, 'Outros': 0 };

    for (const methods of Object.values(swagger.paths)) {
      for (const op of Object.values(methods as any)) {
        const operation = op as { tags?: string[] };
        const tags: string[] = Array.isArray(operation.tags) ? operation.tags : [];

        for (const tag of tags) {
          if (tag.startsWith('Sistema Any Point Store')) contagem['Any Point Store']++;
          else if (/^\d+[\.: -]/.test(tag)) contagem['Integrações']++;
          else contagem['Outros']++;
        }
      }
    }

    this.pieChartDeParaData = {
      labels: Object.keys(contagem),
      datasets: [{ data: Object.values(contagem), backgroundColor: this.getPieColors(3) }]
    };
  }

  async loadDetalhesDasIntegracoesViaSwagger(): Promise<void> {
    const swagger = await this.getSwagger();
    const contagem: Record<string, number> = {};

    for (const methods of Object.values(swagger.paths)) {
      for (const op of Object.values(methods as any)) {
        const operation = op as { tags?: string[] };
        const tags: string[] = Array.isArray(operation.tags) ? operation.tags : [];

        for (const tag of tags) {
          if (/^\d+[\.: -]/.test(tag)) {
            contagem[tag] = (contagem[tag] || 0) + 1;
          }
        }
      }
    }

    const entries = Object.entries(contagem).sort((a, b) => b[1] - a[1]);
    this.pieChartIntegracoesDetalhadasData = {
      labels: entries.map(e => e[0]),
      datasets: [{ data: entries.map(e => e[1]), backgroundColor: this.getPieColors(entries.length) }]
    };
  }

  async loadApisAnyPointStore(): Promise<void> {
    const swagger = await this.getSwagger();
    const contagem: Record<string, number> = {};

    for (const methods of Object.values(swagger.paths)) {
      for (const op of Object.values(methods as Record<string, any>)) {
        const operation = op as { tags?: string[] }; 
        const tags: string[] = Array.isArray(operation.tags) ? operation.tags : [];
        for (const tag of tags) {
          if (tag.startsWith('Sistema Any Point Store')) contagem[tag] = (contagem[tag] || 0) + 1;
        }
      }
    }

    const entries = Object.entries(contagem).sort((a, b) => b[1] - a[1]);
    this.pieChartAnyPointStoreData = {
      labels: entries.map(e => e[0]),
      datasets: [{ data: entries.map(e => e[1]), backgroundColor: this.getPieColors(entries.length) }]
    };
  }

  async getSwagger(): Promise<any> {
    const swaggerBase = environment.api.replace(/\/api$/, '');
    return firstValueFrom(this.http.get<any>(`${swaggerBase}/swagger/v1/swagger.json`));
  }

  getPieColors(count: number): string[] {
    const base = ['#3f51b5', '#e91e63', '#009688', '#ff9800', '#9c27b0', '#2196f3', '#4caf50', '#ff5722', '#00bcd4', '#8bc34a', '#ffc107', '#673ab7'];
    return Array.from({ length: count }, (_, i) => base[i % base.length]);
  }

  irParaDetalhado(): void {
    this.router.navigate(['/dashboard/detalhado']);
  }
  
}
