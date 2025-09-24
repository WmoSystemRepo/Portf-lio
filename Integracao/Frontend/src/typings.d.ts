declare module 'xlsx-style';
declare module 'html2pdf.js';

declare module 'pdfjs-dist/build/pdf.worker.mjs' {
  const worker: any;
  export = worker;
}