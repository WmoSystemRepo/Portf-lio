import { Component, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
// @ts-ignore
import { marked } from 'marked';
import hljs from 'highlight.js';

@Component({
  selector: 'app-documentacao-tecnica',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './documentacao-tecnica.component.html',
  styleUrls: ['./documentacao-tecnica.component.scss'],
})
export class DocumentacaoTecnicaComponent implements AfterViewInit {
  markdownPath = 'assets/Integracao/spp-bimer-invoce/documentacao-tecnica/monitoramento-spp-bimer.md';
  conteudoHtml: SafeHtml | null = null;
  loading = true;
  erro = false;

  // Referência ao container da documentação (para âncoras)
  @ViewChild('markdownContainer', { static: false }) markdownContainer?: ElementRef<HTMLDivElement>;

  constructor(private http: HttpClient, private sanitizer: DomSanitizer) {
    // Configura o highlight.js para blocos de código no markdown
    (marked as any).setOptions({
      highlight: function(code: string, lang: string) {
        if (lang && hljs.getLanguage(lang)) {
          return hljs.highlight(code, { language: lang }).value;
        } else {
          return hljs.highlightAuto(code).value;
        }
      }
    });
    this.carregarMarkdown();
  }

  carregarMarkdown() {
    this.http.get(this.markdownPath, { responseType: 'text' }).subscribe({
      next: (md) => {
        // Corrige os caminhos das imagens relativas (ex: .png)
        const mdCorrigido = md.replace(/\]\((?!http)([^)]+\.png)\)/g,
          '](assets/Integracao/spp-bimer-invoce/documentacao-tecnica/$1)'
        );
        // Converte markdown para HTML
        const html = (marked as any).parse(mdCorrigido) as string;
        this.conteudoHtml = this.sanitizer.bypassSecurityTrustHtml(html);
        this.loading = false;
        // Aguarda renderização e ativa âncoras do índice
        setTimeout(() => this.bindAnchorClicks(), 250);
      },
      error: () => {
        this.erro = true;
        this.loading = false;
      }
    });
  }

  baixarMarkdown() {
    const link = document.createElement('a');
    link.href = this.markdownPath;
    link.download = 'monitoramento-spp-bimer.md';
    link.click();
  }

  ngAfterViewInit() {
    setTimeout(() => this.bindAnchorClicks(), 600);
  }

  /** Ativa rolagem suave para âncoras do índice */
  private bindAnchorClicks() {
    const markdownEl = this.markdownContainer?.nativeElement;
    if (!markdownEl) return;
    const anchors = markdownEl.querySelectorAll('a[href^="#"]');
    anchors.forEach((a) => {
      a.addEventListener('click', (ev) => {
        ev.preventDefault();
        const hash = a.getAttribute('href')?.replace('#', '');
        if (!hash) return;
        // O id pode vir com caracteres escapados em alguns casos. Pode ajustar aqui se necessário!
        const el = markdownEl.querySelector(`[id="${hash}"]`);
        if (el) {
          el.scrollIntoView({ behavior: 'smooth', block: 'start' });
        }
      });
    });
  }
}
