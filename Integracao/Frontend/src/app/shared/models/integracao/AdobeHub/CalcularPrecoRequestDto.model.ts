// src/app/core/models/integracoes/adobe-hub/preco.dto.ts
export interface CalcularPrecoRequestDto {
  fabricanteId: number;
  segmento: string;
  margemBrutaFormulario?: number;
  linhas: {
    partNumber: string;
    fob: number;
    levelDetail?: string | null;
  }[];
}
