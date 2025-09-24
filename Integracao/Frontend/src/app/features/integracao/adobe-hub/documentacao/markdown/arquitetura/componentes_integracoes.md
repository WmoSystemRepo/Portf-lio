# 🧩 Componentes e Integrações

```mermaid
graph TD
    Excel[📄 Planilha Excel]
    Frontend[🖥️ Angular Frontend]
    API[🌐 API .NET Core]
    DB[(🗃️ Banco SQL Server)]
    Regras[vTPRegrasImplementacao]

    Excel --> Frontend
    Frontend --> API
    API --> DB
    API --> Regras
```

## Descrição
- **Excel**: Fonte de dados do usuário
- **Frontend**: Interface para envio do formulário
- **API**: Realiza cálculos, validações e persistência
- **Banco**: Guarda publicações e itens de preços
- **View de Regras**: Define exclusões ou inclusões