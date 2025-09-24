import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TipoTemplate } from '../../../../../../shared/models/anypoint/tipo-template.model';
import { TypeTemplateService } from '../../../../../../core/services/anypoint/templates/type-template.service';
import { GestaoIntegracaoModel } from '../../../../../../shared/models/anypoint/gestao-integracao.model';
import { MenuService } from '../../../../../../core/services/anypoint/menu/menu.service';
import { IntegrationManagementService } from '../../../../../../core/services/anypoint/integration-management/integration-management.service';
import * as XLSX from 'xlsx-js-style';

@Component({
  selector: 'app-registrar-novo-templantes',
  templateUrl: './registrar-novo-templantes.component.html',
  styleUrls: ['./registrar-novo-templantes.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class RegistrarNovoTemplantesComponent implements OnInit {

  @ViewChild('templateSucessoCadastro', { static: false }) templateSucessoCadastroRef!: TemplateRef<any>;
  templateCriado?: TipoTemplate;

  customClass: 'custom-bigblue-rounded-popup' | undefined
  file: File | null = null;
  isDragging = false;
  importButtonText = 'Importar Arquivo';

  tiposTemplate: TipoTemplate[] = [];
  tiposSelecionados: TipoTemplate[] = [];
  liberarVinculo: boolean = false;

  integracaoSelecionada?: GestaoIntegracaoModel;
  integracaoIdAdobeHub?: number;
  integracaoBloqueada = false;

  novoTemplateModalVisible = false;
  novoTemplate: TipoTemplate = {
    nomeCompleto: '',
    sigla: '',
    integracaoId: 0,
    nomeAbreviado: ''
  };

  constructor(
    private router: Router,
    private typeTemplateService: TypeTemplateService,
    private menuService: MenuService,
    private integracaoService: IntegrationManagementService
  ) { }

  ngOnInit(): void {
    this.carregarIntegracaoVinculadaAoMenu();
  }

  private carregarIntegracaoVinculadaAoMenu(): void {

    debugger
    const nomeIntegracaoReferencia = 'ADOBE - HUB';

    this.integracaoService.get().subscribe({

      next: (todasIntegracoes: GestaoIntegracaoModel[]) => {
        const integracaoVinculada = todasIntegracoes.find(
          i => i.nome?.trim().toUpperCase() === nomeIntegracaoReferencia.toUpperCase()
        );

        if (!integracaoVinculada) {
          this.integracaoBloqueada = false;
          this.integracaoIdAdobeHub = undefined;
          return;
        }

        debugger
        this.integracaoSelecionada = integracaoVinculada;
        this.integracaoIdAdobeHub = Number(integracaoVinculada.id);
        this.integracaoBloqueada = false;

        this.typeTemplateService.listar().subscribe({
          next: (tipos: TipoTemplate[]) => {
            this.tiposTemplate = tipos.filter(
              t => t.integracaoId === this.integracaoIdAdobeHub
            );
          },
          error: () => {
            this.tiposTemplate = [];
          }
        });
      },
      error: (err: any) => {
        console.error('❌ Erro ao buscar lista de integrações:', err);
        this.integracaoBloqueada = true;
        Swal.fire({
          icon: 'warning',
          title: 'Atenção!',
          text: 'Erro ao buscar a lista de integrações disponíveis.',
          confirmButtonText: 'OK'
        });
      }
    });
  }

  voltar(): void {
    window.history.back();
  }

  onTipoCheckboxChange(tipo: TipoTemplate, event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked;
    this.tiposSelecionados = isChecked ? [tipo] : [];
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    this.isDragging = false;
    const files = event.dataTransfer?.files;
    if (files && files.length > 0 && !this.integracaoBloqueada) {
      this.handleFile(files[0]);
    }
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0 && !this.integracaoBloqueada) {
      this.handleFile(input.files[0]);
    }
  }

  handleFile(file: File) {
    const extension = file.name.split('.').pop()?.toLowerCase();
    const allowedExtensions = ['csv', 'xls', 'xlsx'];
    if (!extension || !allowedExtensions.includes(extension)) {
      Swal.fire({
        icon: 'warning',
        title: 'Formato não suportado!',
        text: 'Por favor, selecione um arquivo .csv, .xls ou .xlsx.',
        confirmButtonText: 'OK'
      });
      return;
    }

    this.file = file;
    this.importButtonText = extension === 'csv' ? 'Visualizar CSV' : 'Visualizar Arquivo';

    const filename = file.name.toLowerCase();
    const nomeSemExtensao = file.name.replace(/\.[^/.]+$/, '');
    const siglaExtraida = nomeSemExtensao.substring(0, 100);

    this.typeTemplateService.listar().subscribe({
      next: (tipos: TipoTemplate[]) => {
        const templates = tipos.filter(t => t.integracaoId === this.integracaoIdAdobeHub);

        let tipoDetectado = templates.find(tipo =>
          tipo.nomeCompleto?.toLowerCase().includes(nomeSemExtensao.toLowerCase())
        );

        if (!tipoDetectado) {
          tipoDetectado = templates.find(tipo =>
            tipo.nomeAbreviado &&
            filename.includes(tipo.nomeAbreviado.toLowerCase())
          );
        }

        if (tipoDetectado) {
          const tipoComMesmaSigla = templates.find(t => t.sigla === tipoDetectado!.sigla);
          if (tipoComMesmaSigla) {
            this.tiposSelecionados = [tipoComMesmaSigla];
            this.liberarVinculo = true;
          }
          return;
        }

        // ✅ Correção aqui: adiciona 'integracao: null' para evitar erro 400
        this.novoTemplate = {
          nomeCompleto: nomeSemExtensao,
          nomeAbreviado: nomeSemExtensao,
          sigla: siglaExtraida,
          integracaoId: this.integracaoIdAdobeHub ?? 0,
          integracao: this.integracaoSelecionada
        };

        this.novoTemplateModalVisible = true;
      },
      error: () => {
        Swal.fire({
          icon: 'error',
          title: 'Erro',
          text: 'Não foi possível carregar os tipos de template da integração.',
          confirmButtonText: 'OK'
        });
      }
    });
  }

  confirmarCadastroTemplate(): void {
    // ✅ LOG completo do objeto que será enviado no POST
    debugger
    console.log('🔍 Enviando novoTemplate para API:', this.novoTemplate);

    this.typeTemplateService.criar(this.novoTemplate).subscribe({
      next: (created) => {
        this.tiposTemplate.push(created);
        this.novoTemplateModalVisible = false;
        this.templateCriado = created;

        debugger
        const conteudo = document.createElement('div');

        conteudo.innerHTML = `
        <div class="dados-template">
          <strong>Sigla:</strong> ${created.sigla} <br />
          <strong>Nome:</strong> ${created.nomeCompleto} <br />
          <strong>Nome abreviado:</strong> ${created.nomeAbreviado ?? '-'} <br />
        </div>
        <div class="mensagem-destaque">
          Deseja continuar cadastrando o template?
        </div>
      `;

        debugger
        Swal.fire({
          icon: 'success',
          title: 'Tipo de template criado!',
          html: conteudo,
          showCancelButton: true,
          confirmButtonText: 'Sim',
          cancelButtonText: 'Não',
          customClass: {
            popup: 'custom-bigblue-rounded-popup'
          }
        }).then((result) => {
          this.tiposSelecionados = [created];
          this.liberarVinculo = true;

          if (result.isConfirmed) {
            this.processFile();
          }
        });
      },
      error: (error) => {
        console.error('❌ Erro ao criar tipo de template:', error); // ✅ LOG do erro 400
        Swal.fire({
          icon: 'error',
          title: 'Erro ao criar tipo de template',
          text: 'Ocorreu um erro ao tentar salvar o novo tipo de template.',
          confirmButtonText: 'OK'
        });
      }
    });
  }

  clearFile() {
    this.file = null;
    this.importButtonText = 'Importar Arquivo';
    this.tiposSelecionados = [];
  }

  removerArquivo() {
    this.clearFile();
  }

  processFile() {
    const file = this.file;
    if (!file || this.tiposSelecionados.length === 0) {
      Swal.fire({
        icon: 'warning',
        title: 'Atenção!',
        text: 'Selecione um arquivo e ao menos um tipo de template.',
        confirmButtonText: 'OK'
      });
      return;
    }

    const extension = file.name.split('.').pop()?.toLowerCase();
    if (extension === 'xlsm') {
      Swal.fire({
        icon: 'warning',
        title: 'Formato inválido!',
        text: 'O arquivo .xlsm está fora do padrão aceito.',
        confirmButtonText: 'OK'
      });
      return;
    }

    const reader = new FileReader();
    reader.onload = (e: any) => {
      let jsonData: any[][] = [];

      try {
        const fileContent = e.target.result;

        if (extension === 'csv') {
          const csvText = new TextDecoder('utf-8').decode(fileContent);
          const isSemicolon = csvText.includes(';');

          const workbook = XLSX.read(csvText, {
            type: 'string',
            FS: isSemicolon ? ';' : ','
          });

          const sheet = workbook.Sheets[workbook.SheetNames[0]];
          jsonData = XLSX.utils.sheet_to_json(sheet, { header: 1 }) as any[][];
        } else {
          const data = new Uint8Array(fileContent);
          const workbook = XLSX.read(data, { type: 'array' });
          const sheet = workbook.Sheets[workbook.SheetNames[0]];
          jsonData = XLSX.utils.sheet_to_json(sheet, { header: 1 }) as any[][];
        }

        if (!jsonData || jsonData.length === 0) {
          Swal.fire({
            icon: 'warning',
            title: 'Arquivo vazio!',
            text: 'Não foi possível extrair dados do arquivo selecionado.',
            confirmButtonText: 'OK'
          });
          return;
        }

        this.router.navigate(['/Adobe/Novo/Templantes/View'], {
          state: {
            data: jsonData,
            fileName: file.name,
            tipoTemplate: this.tiposSelecionados[0]
          }
        });

      } catch (error) {
        console.error('Erro ao ler o arquivo:', error);

        Swal.fire({
          icon: 'error',
          title: 'Erro ao processar arquivo',
          text: 'Ocorreu um erro ao tentar ler o arquivo. Verifique o formato ou tente outro arquivo.',
          confirmButtonText: 'OK'
        });
      }
    };

    extension === 'csv' ? reader.readAsText(file) : reader.readAsArrayBuffer(file);
  }

  isTipoSelecionado(tipo: TipoTemplate): boolean {
    return this.tiposSelecionados?.some(t => t?.id === tipo?.id);
  }

  cancelarCadastroTemplate(): void {
    this.novoTemplateModalVisible = false;
    this.liberarVinculo = false;
    this.clearFile();
  }

}