import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chart, ChartConfiguration, ChartData } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { BaseChartDirective, NgChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import { RelatoriosService } from '../../../../core/services/intregracoes/vexpenses-bimmer/relatorios.service';
import { GetRelatoriosParams } from '../../../../core/services/intregracoes/vexpenses-bimmer/vexpenses.service';

interface DashboardDto {
  totais: {
    totalPendencias: number;
    totalErros: number;
    totalSucesso: number;
    totalGeral: number;
    totalPago: number;
    totalAprovados: number;
    totalAbertos: number;
    totalEnviados: number;
    totalReabertos: number;
    totalReprovados: number;
  };

  graficoMensal: {
    labels: string[];
    pendencias: number[];
    erros: number[];
    sucesso: number[];
    pendenciasResolvidas: number[];
  };

  graficoPizzaPendencias: {
    labels: string[];
    valores: number[];
  };

  graficoPizzaErros: {
    labels: string[];
    valores: number[];
  };

  graficoPizzaPendenciasExcluidasPorUsario: {
    labels: string[];
    valores: number[];
  }
}

Chart.register(ChartDataLabels);

@Component({
  selector: 'app-dashboard-vexpenses-bimer',
  templateUrl: './dashboard-vexpenser_bimer.component.html',
  styleUrls: ['./dashboard-vexpenser_bimer.component.css'],
  imports: [NgChartsModule, CommonModule]
})
export class DashboarVexpessesBimerdComponent implements OnInit {

  // Flags de carregamento por card
  carregandoPagos = false;
  carregandoAprovados = false;
  carregandoAbertos = false;
  carregandoEnviados = false;
  carregandoReabertos = false;
  carregandoReprovados = false;

  dataUltimaAtualizacaoPagos: string | null = null;
  dataUltimaAtualizacaoAprovados: string | null = null;
  dataUltimaAtualizacaoAbertos: string | null = null;
  dataUltimaAtualizacaoEnviados: string | null = null;
  dataUltimaAtualizacaoReabertos: string | null = null;
  dataUltimaAtualizacaoReprovados: string | null = null;

  @ViewChild('statusFinaisChart') statusFinaisChart?: BaseChartDirective;

  chartPendenciasExcluidasPorUsuariosPizzaData: ChartData<'pie'> = { labels: [], datasets: [] };
  chartPendenciasMoedaPizzaData: ChartData<'pie'> = { labels: [], datasets: [] };
  chartErrosExcecaoPizzaData: ChartData<'pie'> = { labels: [], datasets: [] };

  readonly CORES_STATUS = ['#43a047', '#26c6da', '#9c27b0', '#42a5f5', '#ff9800', '#e53935'];

  constructor(
    private http: HttpClient,
    private router: Router,
    private relatoriosService: RelatoriosService
  ) { }

  ngOnInit(): void {
    this.loadCounts();
    this.loadDashboard();
  }

  barChartUsersOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          color: '#333',
          font: {
            size: 13,
            weight: 500
          },
          padding: 12
        }
      },
      datalabels: {
        anchor: 'end',
        align: 'end',
        color: '#000',
        font: {
          weight: 'bold',
          size: 12
        },
        formatter: (value) => value
      },
      tooltip: {
        backgroundColor: '#fff',
        titleColor: '#000',
        bodyColor: '#000',
        borderColor: '#ccc',
        borderWidth: 1,
        titleFont: { size: 14, weight: 'bold' },
        bodyFont: { size: 13 }
      }
    },
    scales: {
      x: {
        ticks: {
          color: '#222',
          font: { size: 12 }
        },
        grid: {
          color: '#e0e0e0'
        }
      },
      y: {
        ticks: {
          color: '#222',
          font: { size: 12 }
        },
        grid: {
          color: '#e0e0e0'
        },
        beginAtZero: true
      }
    }
  };

  lineChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
        labels: {
          color: '#333',
          font: {
            size: 13
          }
        }
      },
      tooltip: {
        backgroundColor: '#fff',
        titleColor: '#000',
        bodyColor: '#000',
        borderColor: '#ccc',
        borderWidth: 1
      }
    },
    scales: {
      x: {
        ticks: {
          color: '#222',
          font: { size: 12 }
        },
        grid: {
          display: false
        }
      },
      y: {
        ticks: {
          color: '#222',
          font: { size: 12 }
        },
        grid: {
          color: '#e0e0e0'
        },
        beginAtZero: true
      }
    }
  };

  pieChartOptions: ChartConfiguration<'pie'>['options'] = {
    responsive: true,
    plugins: {
      legend: {
        position: 'bottom',
        labels: {
          color: '#333',
          font: { size: 13 },
          boxWidth: 16,
          padding: 8,
          usePointStyle: true
        }
      },
      datalabels: {
        formatter: (value, context) => {
          const total = context.chart.data.datasets[0].data
            .map(v => typeof v === 'number' ? v : 0)
            .reduce((a, b) => a + b, 0);
          return `${((+value / total) * 100).toFixed(1)}%`;
        },
        color: '#fff',
        font: { weight: 'bold', size: 13 }
      },
      tooltip: {
        backgroundColor: '#fff',
        titleColor: '#000',
        bodyColor: '#000',
        borderColor: '#ccc',
        borderWidth: 1
      }
    }
  };

  chartPendenciasData: ChartData<'line'> = { labels: [], datasets: [] };
  chartPendenciasResolvidasData: ChartData<'line'> = { labels: [], datasets: [] };
  chartErrosData: ChartData<'line'> = { labels: [], datasets: [] };
  chartSucessoData: ChartData<'line'> = { labels: [], datasets: [] };
  chartProcessamentoLinhaData: ChartData<'line'> = { labels: [], datasets: [] };
  chartComparativoData: ChartData<'bar'> = { labels: [], datasets: [] };
  chartTotalComparativoData: ChartData<'bar'> = { labels: [], datasets: [] };
  chartPendenciasStatusPizzaData: ChartData<'pie'> = { labels: [], datasets: [] };
  chartErrosPizzaData: ChartData<'pie'> = { labels: [], datasets: [] };

  totalGeral?: number;
  totalPago?: number;
  totalPendencias?: number;
  totalErros?: number;
  totalSucesso?: number;
  totalAprovados?: number;
  totalAbertos?: number;
  totalEnviados?: number;
  totalReabertos?: number;
  totalReprovados?: number;

  loadCounts() {
    this.relatoriosService.getReportCounts().subscribe({
      next: (counts) => {
        this.totalPago = counts.totalPago;
        this.totalAprovados = counts.totalAprovados;
        this.totalAbertos = counts.totalAbertos;
        this.totalReprovados = counts.totalReprovados;
        this.totalReabertos = counts.totalReabertos;
        this.totalEnviados = counts.totalEnviados;
      },
      error: (err) => console.error('Erro ao carregar contagens:', err)
    });
  }

  async loadDashboard(): Promise<void> {
    try {
    
      const response = await firstValueFrom(
        this.http.get<DashboardDto>(`${environment.api}/Integracao/Vexpensses/Bimer/Dashboard`)
      );

      this.chartComparativoData = {
        labels: ['Pendências', 'Erros', 'Sucesso'],
        datasets: [{
          label: 'Total por Categoria',
          data: [
            response.totais.totalPendencias ?? 0,
            response.totais.totalErros ?? 0,
            response.totais.totalSucesso ?? 0
          ],
          backgroundColor: ['#ff9800', '#e53935', '#43a047']
        }]
      };

      this.chartTotalComparativoData = {
        labels: [
          'Total Geral',
          'Pendências',
          'Erros',
          'Sucesso',
          'Pagos',
          'Aprovados',
          'Abertos',
          'Enviados',
          'Reabertos',
          'Reprovados'
        ],
        datasets: [{
          label: 'Processamento Geral',
          data: [
            response.totais.totalGeral ?? 0,
            response.totais.totalPendencias ?? 0,
            response.totais.totalErros ?? 0,
            response.totais.totalSucesso ?? 0,
            response.totais.totalPago ?? 0,
            response.totais.totalAprovados ?? 0,
            response.totais.totalAbertos ?? 0,
            response.totais.totalEnviados ?? 0,
            response.totais.totalReabertos ?? 0,
            response.totais.totalReprovados ?? 0
          ],
          backgroundColor: [
            '#1565c0',
            '#ff9800',
            '#e53935',
            '#43a047',
            '#7cb342',
            '#00acc1',
            '#8e24aa',
            '#1e88e5',
            '#fdd835',
            '#d81b60'
          ]
        }]
      };

      this.chartPendenciasStatusPizzaData = {
        labels: response.graficoPizzaPendencias.labels,
        datasets: [{
          data: response.graficoPizzaPendencias.valores,
          backgroundColor: ['#42a5f5', '#e53935', '#ffa726', '#ab47bc']
        }]
      };

      this.chartErrosPizzaData = {
        labels: response.graficoPizzaErros.labels,
        datasets: [{
          data: response.graficoPizzaErros.valores,
          backgroundColor: ['#ab47bc', '#26a69a', '#ffa726']
        }]
      };

      this.chartPendenciasData = {
        labels: response.graficoMensal.labels,
        datasets: [this.criarLinha('Pendências', response.graficoMensal.pendencias, '#ff9800')]
      };

      this.chartErrosData = {
        labels: response.graficoMensal.labels,
        datasets: [this.criarLinha('Erros', response.graficoMensal.erros, '#e53935')]
      };

      this.chartSucessoData = {
        labels: response.graficoMensal.labels,
        datasets: [this.criarLinha('Sucesso', response.graficoMensal.sucesso, '#43a047')]
      };


      this.chartProcessamentoLinhaData = {
        labels: response.graficoMensal.labels,
        datasets: [
          this.criarLinha('Pendências', response.graficoMensal.pendencias, '#ff9800'),
          this.criarLinha('Erros', response.graficoMensal.erros, '#e53935'),
          this.criarLinha('Sucesso', response.graficoMensal.sucesso, '#43a047')
        ]
      };

      this.chartPendenciasResolvidasData = {
        labels: response.graficoMensal.labels,
        datasets: [this.criarLinha('Pendências Resolvidas', response.graficoMensal.pendenciasResolvidas, '#26c6da')]
      };

      if (response.graficoPizzaPendenciasExcluidasPorUsario) {
      
        this.chartPendenciasExcluidasPorUsuariosPizzaData = {
          labels: response.graficoPizzaPendenciasExcluidasPorUsario.labels || [],
          datasets: [{
            data: response.graficoPizzaPendenciasExcluidasPorUsario.valores || [],
            backgroundColor: ['#26c6da', '#ff9800', '#7e57c2', '#ff7043']
          }]
        };
      }

    } catch (error) {
      console.error('Erro ao carregar o dashboard', error);
    }
  }

  private criarLinha(label: string, data: number[], cor: string) {
    return {
      data,
      label,
      borderColor: cor,
      backgroundColor: cor,
      fill: false
    };
  }

  irParaDetalhado(): void {
    this.router.navigate(['/dashboard/detalhado']);
  }

  onClickCardPago(event: Event): void {
    this.carregandoPagos = true;
    const params: GetRelatoriosParams = {
      pageNumber: 1,
      pageSize: 10,
      status: 'PAGO',
      search: '',
      searchField: 'approval_date_between',
      searchJoin: 'and'
    };
    this.relatoriosService.getRelatoriosCards(params).subscribe({
      next: () => {
        this.dataUltimaAtualizacaoPagos = this.getDataHoraAtual();
        this.loadCounts(); // 🔁 Atualiza os totais na tela
        this.carregandoPagos = false;
      },
      error: () => {
        this.carregandoPagos = false;
      }
    });
  }

  onClickCardAprovados(event: Event): void {
    this.carregandoAprovados = true;
    const params: GetRelatoriosParams = {
      pageNumber: 1,
      pageSize: 10,
      status: 'APROVADO',
      search: '',
      searchField: 'approval_date_between',
      searchJoin: 'and'
    };
    this.relatoriosService.getRelatoriosCards(params).subscribe({
      next: () => {
        this.dataUltimaAtualizacaoAprovados = this.getDataHoraAtual();
        this.loadCounts(); // 🔁 Atualiza os totais na tela
        this.carregandoAprovados = false;
      },
      error: () => {
        this.carregandoAprovados = false;
      }
    });
  }

  onClickCardAbertos(event: Event): void {
    this.carregandoAbertos = true;
    const params: GetRelatoriosParams = {
      pageNumber: 1,
      pageSize: 10,
      status: 'ABERTO',
      search: '',
      searchField: 'approval_date_between',
      searchJoin: 'and'
    };
    this.relatoriosService.getRelatoriosCards(params).subscribe({
      next: () => {
        this.dataUltimaAtualizacaoAbertos = this.getDataHoraAtual();
        this.loadCounts(); // 🔁 Atualiza os totais na tela
        this.carregandoAbertos = false;
      },
      error: () => {
        this.carregandoAbertos = false;
      }
    });
  }

  onClickCardEnviados(event: Event): void {
    this.carregandoEnviados = true;
    const params: GetRelatoriosParams = {
      pageNumber: 1,
      pageSize: 10,
      status: 'ENVIADO',
      search: '',
      searchField: 'approval_date_between',
      searchJoin: 'and'
    };
    this.relatoriosService.getRelatoriosCards(params).subscribe({
      next: () => {
        this.dataUltimaAtualizacaoEnviados = this.getDataHoraAtual();
        this.loadCounts(); // 🔁 Atualiza os totais na tela
        this.carregandoEnviados = false;
      },
      error: () => {
        this.carregandoEnviados = false;
      }
    });
  }

  onClickCardReabertos(event: Event): void {
    this.carregandoReabertos = true;
    const params: GetRelatoriosParams = {
      pageNumber: 1,
      pageSize: 10,
      status: 'REABERTO',
      search: '',
      searchField: 'approval_date_between',
      searchJoin: 'and'
    };
    this.relatoriosService.getRelatoriosCards(params).subscribe({
      next: () => {
        this.dataUltimaAtualizacaoReabertos = this.getDataHoraAtual();
        this.loadCounts(); // 🔁 Atualiza os totais na tela
        this.carregandoReabertos = false;
      },
      error: () => {
        this.carregandoReabertos = false;
      }
    });
  }

  onClickCardReprovados(event: Event): void {
    this.carregandoReprovados = true;
    const params: GetRelatoriosParams = {
      pageNumber: 1,
      pageSize: 10,
      status: 'REPROVADO',
      search: '',
      searchField: 'approval_date_between',
      searchJoin: 'and'
    };
    this.relatoriosService.getRelatoriosCards(params).subscribe({
      next: () => {
        this.dataUltimaAtualizacaoReprovados = this.getDataHoraAtual();
        this.loadCounts(); // 🔁 Atualiza os totais na tela
        this.carregandoReprovados = false;
      },
      error: () => {
        this.carregandoReprovados = false;
      }
    });
  }

  getDataHoraAtual(): string {
    const agora = new Date();
    const data = agora.toLocaleDateString('pt-BR');
    const hora = agora.toLocaleTimeString('pt-BR', { hour12: false });
    return `${data}, ${hora}`;
  }
}