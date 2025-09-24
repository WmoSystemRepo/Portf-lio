// dashboard-bacem-dolar.component.ts
import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Chart, Filler, ChartData, ChartOptions } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatOptionModule } from '@angular/material/core';
import { environment } from '../../../../../environments/environment';

Chart.register(Filler);

@Component({
  selector: 'app-dashboard-bacem-dolar',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatOptionModule,
    NgChartsModule,
  ],
  templateUrl: './dashboard-bacem-dolar.component.html',
  styleUrls: ['./dashboard-bacem-dolar.component.scss']
})
export class DashboardBacemDolarComponent implements OnInit {
  dollarRate: any;
  margem: number = 0;
  allHistoricalData: any[] = [];
  form!: FormGroup;
  private readonly fb = inject(FormBuilder);

  historicalData: number[] = [];
  labels: string[] = [];
  selectedDays = 7;
  activeRange = 7;

  lineChartData!: ChartData<'line'>;
  lineChartOptions!: ChartOptions<'line'>;

  cotacaoAnterior: any = null;
  cotacaoDirecao: 'Subiu' | 'Desceu' | 'Estável' | null = null;

  moedasFixas = [
    { codigo: 'USD', nome: 'Dólar Americano' },
    { codigo: 'EUR', nome: 'Euro' },
    { codigo: 'ARS', nome: 'Peso Argentino' },
  ];

  selectedCurrency: string = 'USD';
  private baseUrl = environment.api;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    const end = new Date();
    const start = new Date();
    start.setDate(end.getDate() - this.selectedDays);

    this.form = this.fb.group({
      moeda: [this.selectedCurrency, Validators.required],
      dataInicio: [start.toISOString().slice(0, 10), Validators.required],
      dataFim: [end.toISOString().slice(0, 10), Validators.required],
    });

    this.fetchCurrentQuote();
    this.applyDateRangeFilter(this.selectedDays);
  }

  fetchCurrentQuote(): void {
    this.http.get<any>(`${this.baseUrl}/cambio/listagem`).subscribe(data => {
      if (!data || !Array.isArray(data.historico)) return;

      this.dollarRate = data.cotacaoAtual;

      if (this.dollarRate?.cotacaoVenda && this.dollarRate?.cotacaoCompra) {
        this.margem = this.dollarRate.cotacaoVenda - this.dollarRate.cotacaoCompra;
      }

      if (this.cotacaoAnterior) {
        const atual = this.dollarRate.cotacaoVenda;
        const anterior = this.cotacaoAnterior.cotacaoVenda;
        this.cotacaoDirecao = atual > anterior ? 'Subiu' : atual < anterior ? 'Desceu' : 'Estável';
      }
      this.cotacaoAnterior = this.dollarRate;

      const sortedData = data.historico.sort((a: any, b: any) =>
        new Date(a.dataHoraCotacao).getTime() - new Date(b.dataHoraCotacao).getTime()
      );

      this.historicalData = sortedData.map((v: any) => v.cotacaoVenda);
      this.allHistoricalData = sortedData;
      this.labels = sortedData.map((v: any) =>
        new Date(v.dataHoraCotacao).toLocaleDateString('pt-BR')
      );

      this.setupChart();
    });
  }

  applyDateRangeFilter(days: number): void {
    const cutoffDate = new Date();
    cutoffDate.setDate(cutoffDate.getDate() - days);

    const filtered = this.allHistoricalData.filter(item =>
      new Date(item.dataHoraCotacao) >= cutoffDate
    );

    this.historicalData = filtered.map((v: any) => v.cotacaoVenda);
    this.labels = filtered.map((v: any) =>
      new Date(v.dataHoraCotacao).toLocaleDateString('pt-BR')
    );

    this.setupChart();
  }

  changeRange(days: number): void {
    this.selectedDays = days;
    this.activeRange = days;
    this.applyDateRangeFilter(days);
  }

  buscarCotacoes(): void {
    const { dataInicio, dataFim, moeda } = this.form.value;
    if (!dataInicio || !dataFim || !moeda) return;

    const url = `${this.baseUrl}/cambio/cotacao-dolar?dataInicio=${dataInicio}&dataFim=${dataFim}&codigoMoeda=${moeda}`;

    this.http.get<any[]>(url).subscribe(data => {
      const sortedData = data.sort((a: any, b: any) =>
        new Date(a.dataHoraCotacao).getTime() - new Date(b.dataHoraCotacao).getTime()
      );

      this.historicalData = sortedData.map(v => v.cotacaoVenda);
      this.labels = sortedData.map(v =>
        new Date(v.dataHoraCotacao).toLocaleDateString('pt-BR')
      );

      this.setupChart();
    });
  }

  fetchHistoricalData(days: number): void {
    const endDate = new Date();
    const startDate = new Date();
    startDate.setDate(endDate.getDate() - days);

    const dataInicio = startDate.toISOString();
    const dataFim = endDate.toISOString();

    const url = `${this.baseUrl}/cambio/cotacao-dolar?dataInicio=${dataInicio}&dataFim=${dataFim}&codigoMoeda=${this.selectedCurrency}`;

    this.http.get<any[]>(url).subscribe(data => {
      const sortedData = data.sort((a: any, b: any) =>
        new Date(a.dataHoraCotacao).getTime() - new Date(b.dataHoraCotacao).getTime()
      );

      this.historicalData = sortedData.map((v: any) => v.cotacaoVenda);
      this.labels = sortedData.map((v: any) =>
        new Date(v.dataHoraCotacao).toLocaleDateString('pt-BR')
      );

      this.setupChart();
    });
  }

  onCurrencyChange(): void {
    this.fetchHistoricalData(this.selectedDays);
  }

  setupChart(): void {
    if (!this.historicalData?.length || !this.labels?.length) return;

    this.lineChartData = {
      labels: this.labels,
      datasets: [
        {
          data: this.historicalData,
          label: 'USD/BRL',
          borderColor: '#00e676',
          backgroundColor: 'rgba(0, 230, 118, 0.2)',
          fill: true,
          tension: 0.4,
          pointRadius: 5,
          pointHoverRadius: 8
        }
      ]
    };

    this.lineChartOptions = {
      responsive: true,
      plugins: {
        legend: { display: false },
        tooltip: {
          backgroundColor: '#1f1f1f',
          titleColor: '#00e676',
          bodyColor: '#ffffff'
        }
      },
      scales: {
        x: { ticks: { color: '#cccccc' } },
        y: { ticks: { color: '#cccccc' } }
      }
    };
  }
}
