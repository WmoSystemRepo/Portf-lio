import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CalcularPrecoRequestDto } from '../../../../shared/models/integracao/AdobeHub/CalcularPrecoRequestDto.model';
import { CalcularPrecoResponseDto } from '../../../../shared/models/integracao/AdobeHub/CalcularPrecoResponseDto.model';

//#region Interface
export interface ConfiguracoesResponseDto {
  metodoMargemAdobe: string;
  margemFixa?: number | null;
  pis: number;
  cofins: number;
  iss: number;
  custoOperacional: number;
  prodNivel1: number;
  outrosProd: number;
  margemMinima?: number | null;
}
//#endregion

@Injectable({ providedIn: 'root' })
export class AdobePrecoApiService {
  private readonly base = '/api/adobe';

  //#region Constructor
  constructor(private http: HttpClient) {}
  //#endregion

  //#region Buscar Configuração
  getConfiguracoes(fabricanteId: number, segmento: string): Observable<ConfiguracoesResponseDto> {
    const params = new HttpParams()
      .set('fabricanteId', fabricanteId)
      .set('segmento', segmento);
    return this.http.get<ConfiguracoesResponseDto>(`${this.base}/configuracoes`, { params });
  }
  //#endregion

  //#region Calcular o Preço
  calcularPrecos(dto: CalcularPrecoRequestDto): Observable<CalcularPrecoResponseDto> {
    return this.http.post<CalcularPrecoResponseDto>(`${this.base}/calcular`, dto);
  }
  //#endregion
}