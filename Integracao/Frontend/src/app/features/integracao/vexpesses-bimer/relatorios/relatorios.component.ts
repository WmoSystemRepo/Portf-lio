import {
  Component,
  ViewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { RelatorioMonitorIntegracaoComponent } from '../monitor-integracao/relatorio-monitor-integracao.component';
import { RelatorioMonitorPendenciasComponent } from '../monitor-pendencias/relatorio-monitor-pendencias.component';
import { RelatorioMonitorErrosComponent } from '../monitor-erros/relatorio-monitor-erros.component';

@Component({
    selector: 'app-relatorios',
    imports: [
        CommonModule,
        MatIconModule,
        RelatorioMonitorIntegracaoComponent,
        RelatorioMonitorPendenciasComponent,
        RelatorioMonitorErrosComponent,
    ],
    templateUrl: './relatorios.component.html',
    styleUrls: ['./relatorios.component.css']
})
export class RelatoriosComponent {
  abaSelecionada: 'integracao' | 'pendencias' | 'erros' = 'integracao';

  // 🔍 ViewChild para acessar o componente de erros
  @ViewChild(RelatorioMonitorErrosComponent)
  monitorErrosComponent?: RelatorioMonitorErrosComponent;

  /**
   * Altera a aba ativa e carrega os dados de erros somente quando necessário
   */
  selecionar(aba: 'integracao' | 'pendencias' | 'erros') {
    this.abaSelecionada = aba;

    if (aba === 'erros') {
      setTimeout(() => {
        this.monitorErrosComponent?.carregarErros();
      });
    }
  }
}
