export interface CalcularPrecoResponseDto {
  linhas: {
    partNumber: string;
    precoRevendaUS: number;
    observacao?: string | null;
  }[];
}