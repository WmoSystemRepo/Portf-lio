import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MarkdownModule } from 'ngx-markdown';

@Component({
  standalone: true,
  selector: 'app-markdown-viewer',
  templateUrl: './markdown-viewer.component.html',
  styleUrls: ['./markdown-viewer.component.scss'],
  imports: [
    CommonModule,
    HttpClientModule,
    MarkdownModule
  ]
})
export class MarkdownViewerComponent implements OnInit {

  //#region Propriedades
  markdownPath: string = '';
  //#endregion

  //#region Construtor
  constructor(private route: ActivatedRoute) {}
  //#endregion

  //#region Ciclo de vida
  ngOnInit(): void {
    const nomeArquivo = this.route.snapshot.paramMap.get('arquivo');
    this.markdownPath = `assets/documentacao-markdown/${nomeArquivo}.md`;
  }
  //#endregion
}