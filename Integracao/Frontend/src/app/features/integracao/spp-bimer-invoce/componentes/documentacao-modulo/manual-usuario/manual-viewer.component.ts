import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PdfViewerModule } from 'ng2-pdf-viewer';

@Component({
  selector: 'app-manual-viewer',
  standalone: true,
  imports: [CommonModule, PdfViewerModule],
  templateUrl: './manual-viewer.component.html',
  styleUrls: ['./manual-viewer.component.scss'],
})
export class ManualViewerComponent {
  contentHtml: string = '';

  pdfSrc = '/assets/Integracao/spp-bimer-invoce/manual-usuario/manual-usuario-monitor.pdf';
  page = 1;
  totalPages?: number;

  onPdfLoaded(pdf: any) {
    this.totalPages = pdf.numPages;
  }

  nextPage() {
    if (this.page < (this.totalPages || 0)) {
      this.page++;
    }
  }

  prevPage() {
    if (this.page > 1) {
      this.page--;
    }
  }

  downloadPdf() {
    const link = document.createElement('a');
    link.href = this.pdfSrc;
    link.download = 'Manual-do-Usuario-Monitor.pdf';
    link.target = '_blank';
    link.click();
  }
}
