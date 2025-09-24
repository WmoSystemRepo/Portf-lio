# 🧠 ADR – Architecture Decision Records

## ADR-001 — Uso de "MetodoMargemAdobe"
- **Decisão:** Usar margem por nível (`ProdNivel1` e `OutrosProd`) se valor for `'N'`.
- **Motivo:** Necessidade de aplicar regras distintas para produtos específicos da Adobe.
- **Data:** 2025-08-15

## ADR-002 — Validação de `PartNumber` fora de segmentos
- **Decisão:** Apenas validar formato de `PartNumber` se `SegmentoFabricante` não for de exceção.
- **Motivo:** Produtos dos segmentos `ET`, `FL`, `DM` e similares não possuem validação obrigatória.
- **Data:** 2025-08-15