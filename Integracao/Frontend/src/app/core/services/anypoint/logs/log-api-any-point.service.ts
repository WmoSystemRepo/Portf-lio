import { Injectable } from '@angular/core';
import { ApiLogAnyPoint } from '../../../../shared/models/anypoint/apiLogAnyPoint.model';

@Injectable({
  providedIn: 'root'
})
export class LogApiAnyPointService {
  private logs: ApiLogAnyPoint[] = [];

  //#region LOGS ANY POINT 
  registrarLog(log: ApiLogAnyPoint): void {
    this.logs.push(log);
  }

  obterLogs(): ApiLogAnyPoint[] {
    return this.logs;
  }

  obterLogsRecentes(limit: number = 20): ApiLogAnyPoint[] {
    return this.logs.slice(-limit).reverse();
  }

  obterLogsComErro(): ApiLogAnyPoint[] {
    return this.logs.filter(log => !log.sucesso);
  }

  limparLogs(): void {
    this.logs = [];
  }
  //#endregion
}
