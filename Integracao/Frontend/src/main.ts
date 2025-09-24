import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { provideAnimations } from '@angular/platform-browser/animations';
import { JoyrideModule } from 'ngx-joyride';
import { AppComponent } from './app/shared/app.component';
import { routes } from './app/app.routes';
import { Chart } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { provideNativeDateAdapter } from '@angular/material/core';
import { AuthInterceptorFn } from './app/core/interceptors/AuthInterceptorFn';
import * as pdfjsLib from 'pdfjs-dist';
import * as pdfjsWorker from 'pdfjs-dist/build/pdf.worker.mjs';

(pdfjsLib as any).GlobalWorkerOptions.workerSrc = 
  new URL('pdfjs-dist/build/pdf.worker.mjs', import.meta.url).toString();

Chart.register(ChartDataLabels);
bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([AuthInterceptorFn])),
    provideAnimations(),
    provideNativeDateAdapter(),
    importProvidersFrom(JoyrideModule.forRoot())
  ]
}).catch(err => console.error(err));