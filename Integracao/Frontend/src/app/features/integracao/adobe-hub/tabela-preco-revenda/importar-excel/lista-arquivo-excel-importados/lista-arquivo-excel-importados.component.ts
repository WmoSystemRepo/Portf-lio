import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { forkJoin, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ListaCrudIntegracaoComponent } from '../../../../../../shared/components/integracao/lista-crud-importacao/lista-crud-integracao.component';
import * as XLSX from 'xlsx-js-style';
import * as FileSaver from 'file-saver';
import { PlanilhasImportadasModel } from '../../../../../../shared/models/integracao/AdobeHub/planilhas-importadas.model';
import { TemplateModel } from '../../../../../../shared/models/integracao/AdobeHub/templante.model';
import { AuthService } from '../../../../../anypoint/auth';
import { PlaninhaImportadasService } from '../../../../../../core/services/intregracoes/adobe-hub/planinha-importadas.service';
import { AdobePrecoApiService } from '../../../../../../core/services/intregracoes/adobe-hub/adobe-preco-api.service';
import { CalcularPrecoRequestDto } from '../../../../../../shared/models/integracao/AdobeHub/CalcularPrecoRequestDto.model';
import { CalcularPrecoResponseDto } from '../../../../../../shared/models/integracao/AdobeHub/CalcularPrecoResponseDto.model';
import { TipoTemplate } from '../../../../../../shared/models/anypoint/tipo-template.model';

@Component({
  selector: 'app-lista-arquivos-importados',
  standalone: true,
  imports: [CommonModule, ListaCrudIntegracaoComponent],
  templateUrl: './lista-arquivo-excel-importados.component.html'
})
export class ListaArquivosImportadosComponent implements OnInit {

  model: PlanilhasImportadasModel[] = [];
  selectedItems: PlanilhasImportadasModel[] = [];
  versao?: number;

  semResultados = false;
  mensagemVazio = 'Nenhuma planilha importada encontrada.';

  @Output() viewExcel = new EventEmitter<PlanilhasImportadasModel>();

  constructor(
    private service: PlaninhaImportadasService,
    private authService: AuthService,
    private router: Router,
    private precoApi: AdobePrecoApiService
  ) { }

  campos: {
    name: string;
    label: string;
    columnClass?: string;
    align?: 'left' | 'center' | 'right';
  }[] = [
      { name: 'versao', label: 'Versão', columnClass: 'col-md-1', align: 'center' },
      { name: 'template', label: 'Template', columnClass: 'col-md-2', align: 'left' },
      { name: 'nomeArquivo', label: 'Nome Arquivo Base', columnClass: 'col-md-3', align: 'left' },
      { name: 'dataUpload', label: 'Data Upload', columnClass: 'col-md-3', align: 'center' },
      { name: 'usuario', label: 'Importado por', columnClass: 'col-md-3', align: 'left' },
    ];

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.service.ObterPlaninhas()
      .pipe(
        catchError(() => {
          this.model = [];
          this.semResultados = true;
          return of<PlanilhasImportadasModel[]>([]);
        })
      )
      .subscribe((data: PlanilhasImportadasModel[]) => {
        this.model = data ?? [];
        this.semResultados = (this.model.length === 0);
      });
  }

  add(): void {
    const usuario = this.authService.getDecodedToken()?.Name ?? 'Desconhecido';
    this.router.navigate(['/Adobe/Planilhas/Importacao'], { state: { usuario } });
  }

  edit(regra: TemplateModel): void {
    this.router.navigate(['/Planilhas/Editar', regra.id]);
  }

  onDeleteWithJustification(event: { item: PlanilhasImportadasModel, justification: string }): void {
    const { item, justification } = event;

    const user = this.authService.getDecodedToken();
    const payload = {
      justificativa: justification,
      usuario: user.Name,
      dataHora: new Date().toISOString(),
      registrosExcluidos: [
        {
          id: item.id,
          tipo: item.template.tipoTemplate,
          nomeArquivo: item.nomeArquivo,
          dataUpload: item.dataUpload,
          dados: item.dados
        }
      ]
    };

    this.service.ExcluirPLanilhaImportada(payload).subscribe({
      next: (res: any) => {
        this.carregarDadosNovamente();
        Swal.fire('Sucesso', res.message || 'Exclusão realizada com sucesso.', 'success');
      },
      error: () => Swal.fire('Erro', 'Erro ao excluir a planilha.', 'error')
    });
  }

  carregarDadosNovamente(): void {
    window.location.reload();
  }

  startTour(): void {
    Swal.fire('Tour', 'Aqui você pode implementar seu passo-a-passo.', 'info');
  }

  verExcel(item: PlanilhasImportadasModel): void {
    this.service.ObterPlanilhaPorId(item.id).subscribe((planilhaCompleta: PlanilhasImportadasModel) => {
      this.router.navigate(['/Montar/Planilha/Excel'], {
        state: {
          dados: planilhaCompleta.dados,
          tipo: planilhaCompleta.template.tipoTemplate,
          fileName: planilhaCompleta.nomeArquivo,
          selectedTemplate: planilhaCompleta.template.tipoTemplate,
          templateModel: planilhaCompleta.template,
        },
      });
    });
  }

  onSelectedItemsChanged(items: PlanilhasImportadasModel[]): void {
    this.selectedItems = items;
  }

  // ----------------- helpers de alias/parse -----------------
  private getField(obj: any, keys: string[]): any {
    for (const k of keys) {
      if (obj[k] !== undefined && obj[k] !== null && obj[k] !== '') return obj[k];
      const k2 = k.replace(/\s+/g, '');
      if (obj[k2] !== undefined && obj[k2] !== null && obj[k2] !== '') return obj[k2];
    }
    return '';
  }

  private parseDecimal(val: any): number {
    if (val === null || val === undefined || val === '') return 0;
    const s = String(val).replace(/\./g, '').replace(',', '.');
    const n = Number(s);
    return isNaN(n) ? 0 : n;
  }
  // ----------------------------------------------------------

  exportSelectedToExcel(): void {
    if (!this.selectedItems.length) {
      Swal.fire('Atenção', 'Nenhum item selecionado para exportar.', 'warning');
      return;
    }

    const observables = this.selectedItems.map((item) => this.service.ObterPlanilhaPorId(item.id));

    forkJoin(observables).subscribe({
      next: (planilhas: PlanilhasImportadasModel[]) => {
        const request: CalcularPrecoRequestDto = {
          fabricanteId: 1,
          segmento: this.resolverSegmentoGeral(planilhas),
          margemBrutaFormulario: undefined,
          linhas: []
        };

        for (const p of planilhas) {
          if (!Array.isArray(p.dados)) continue;
          for (const r of p.dados) {
            const pn = String(
              this.getField(r, ['Part Number', 'PartNumber', 'PART NUMBER'])
            ).trim();
            if (!pn) continue;

            // ✅ MOD 1: FOB passa a vir prioritariamente de "Partner Price"
            const fobFonte = this.getField(r, [
              'Partner Price', 'PartnerPrice',                 // fonte oficial
              'FOB', 'Fob', 'FOB (US$)'                       // fallback (casos antigos)
            ]);
            const fob = this.parseDecimal(fobFonte);

            const level = this.getField(r, ['Level Detail', 'LevelDetail', 'Level detail']) || null;

            request.linhas.push({ partNumber: pn, levelDetail: level, fob });
          }
        }

        if (!request.linhas.length) {
          Swal.fire('Atenção', 'Nenhum registro válido para calcular.', 'warning');
          return;
        }

        this.precoApi.calcularPrecos(request).subscribe({
          next: (resp: CalcularPrecoResponseDto) => {
            const map = new Map<string, number>();
            for (const l of resp.linhas) {
              map.set((l.partNumber ?? '').trim(), l.precoRevendaUS ?? 0);
            }
            this.gerarExcel(planilhas, map);
          },
          error: () => {
            Swal.fire('Erro', 'Falha ao calcular os preços no servidor.', 'error');
          }
        });
      },
      error: () => Swal.fire('Erro', 'Falha ao carregar as planilhas selecionadas.', 'error')
    });
  }

  private resolverSegmentoGeral(planilhas: PlanilhasImportadasModel[]): string {
    const segs = new Set<string>();
    for (const p of planilhas) {
      const s = (p as any)?.segmentoFabricante || p?.template.tipoTemplate || '';
      if (s) segs.add(String(s));
    }
    return segs.size > 1 ? 'MIX' : (Array.from(segs)[0] ?? 'COM');
  }

  private gerarExcel(planilhas: PlanilhasImportadasModel[], precos?: Map<string, number>): void {
    const now = new Date();
    const dataHora = now
      .toLocaleString('pt-BR', {
        day: '2-digit', month: '2-digit', year: 'numeric',
        hour: '2-digit', minute: '2-digit',
      })
      .replace(',', '');

    const titulo = `TABELA PREÇOS ADOBE ${dataHora}`;
    const segmentosSelecionados = Array.from(new Set(planilhas.map((i) => i.template.tipoTemplate?.sigla)));
    const segmento = `SEGMENTO:   |   ${segmentosSelecionados.join('   |   ')}`;

    const cabecalho: string[][] = [
      [titulo],
      [segmento],
      [''],
      [
        'Part Number', 'Licenças', 'Versão', 'Plataforma', 'Configuração', 'Idioma',
        'Detalhe 1', 'Detalhe 2', 'Duração', 'Usuários', 'Nível', 'Pontos',
        'FOB', 'Preço Revenda(US$)', '3YR',
      ],
    ];

    const corpo: any[][] = planilhas.flatMap((item) => {

      const is3YR = this.is3YRTemplate(item.template.tipoTemplate);

      if (!item.dados || !Array.isArray(item.dados) || !item.dados.length) {
        return [[...Array(15).fill('Sem dados')]];
      }

      return item.dados.map((registro: any) => {
        const pn = String(this.getField(registro, ['Part Number', 'PartNumber', 'PART NUMBER'])).trim();

        const pontos = this.getField(registro, ['Points', 'Pontos']);

        // ✅ MOD 2: FOB exibido também vem de "Partner Price" (com fallback)
        const fobRaw = this.getField(registro, [
          'Partner Price', 'PartnerPrice',                  // fonte oficial
          'FOB', 'Fob', 'FOB (US$)'                         // fallback
        ]);

        const precoPlanilha = this.getField(registro, [
          'Preço Revenda(US$)', 'Preco Revenda(US$)', 'PrecoRevendaUS', 'PrecoRevenda', 'PreçoRevendaUS'
        ]);

        const precoApi = precos?.get(pn);
        const precoFinal = (precoApi !== undefined ? precoApi : this.parseDecimal(precoPlanilha));

        return [
          pn,
          registro['Product Family'] ?? '',
          registro['Version'] ?? '',
          registro['Operating System'] ?? '',
          registro['Product Type'] ?? '',
          registro['Language'] ?? '',
          registro['Product Type Detail'] ?? '',
          registro['Additional Detail'] ?? '',
          registro['Duration'] ?? '',
          registro['Users'] ?? '',
          this.getField(registro, ['Level Detail', 'LevelDetail', 'Level detail']) ?? '',
          pontos ?? '',
          this.formatarNumeroParaVirgula(fobRaw),
          this.formatarNumeroParaVirgula(precoFinal),
          is3YR ? 'Sim' : 'Não',
        ];
      });
    });

    const planilha = XLSX.utils.aoa_to_sheet([...cabecalho, ...corpo]);

    planilha['!merges'] = [
      { s: { r: 0, c: 0 }, e: { r: 0, c: 14 } },
      { s: { r: 1, c: 0 }, e: { r: 1, c: 14 } },
      { s: { r: 2, c: 0 }, e: { r: 2, c: 14 } },
    ];

    planilha['!cols'] = cabecalho[3].map((header) => ({ wch: header.length + 5 }));

    for (let col = 0; col < 15; col++) {
      const cellRef = XLSX.utils.encode_cell({ r: 3, c: col });
      if (planilha[cellRef]) {
        planilha[cellRef].s = {
          font: { bold: true, color: { rgb: '000000' } },
          fill: { fgColor: { rgb: '00B0F0' } },
          alignment: { vertical: 'center', horizontal: 'center' },
        };
      }
    }

    const primeiraLinhaDados = 4;
    const totalLinhas = primeiraLinhaDados + corpo.length;

    for (let row = primeiraLinhaDados; row < totalLinhas; row++) {
      [9, 12].forEach((col) => {
        const cellRef = XLSX.utils.encode_cell({ r: row, c: col });
        if (planilha[cellRef]) {
          planilha[cellRef].s = { alignment: { horizontal: 'center' } };
        }
      });
    }

    planilha[XLSX.utils.encode_cell({ r: 0, c: 0 })].s = {
      font: { bold: true, sz: 14 },
      alignment: { horizontal: 'center' },
    };
    planilha[XLSX.utils.encode_cell({ r: 1, c: 0 })].s = {
      font: { bold: true },
      alignment: { horizontal: 'center' },
    };

    const workbook: XLSX.WorkBook = { Sheets: { Dados: planilha }, SheetNames: ['Dados'] };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'array', cellStyles: true });

    const nomeArquivo = `exportacao_excel_${now.toISOString().slice(0, 10)}.xlsx`;
    FileSaver.saveAs(new Blob([excelBuffer], { type: 'application/octet-stream' }), nomeArquivo);
  }

  private formatarNumeroParaVirgula(valor: any): string {
    if (valor === null || valor === undefined || valor === '') return '';
    const valorStr = String(valor).trim();
    if (!valorStr) return '';
    const numeroRegex = /^-?\d+(\.\d+)?$/;
    if (numeroRegex.test(valorStr)) return valorStr.replace('.', ',');
    const numeroVirgulaBrasileira = /^-?\d+(,\d+)?$/;
    if (numeroVirgulaBrasileira.test(valorStr)) return valorStr;
    const numero = parseFloat(valorStr);
    if (!isNaN(numero)) return numero.toString().replace('.', ',');
    return valorStr;
  }

  private is3YRTemplate(template?: TipoTemplate): boolean {
    const termo = '3YR';

    return !!(
      template?.nomeCompleto?.toUpperCase().includes(termo) ||
      template?.nomeAbreviado?.toUpperCase().includes(termo) ||
      template?.sigla?.toUpperCase().includes(termo)
    );
  }

  onAddProductsToSelected(_e: any) {
    throw new Error('Method not implemented.');
  }

}
