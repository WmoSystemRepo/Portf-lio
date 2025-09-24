# 🪵 Modelo de Logs e Auditoria

## Logs de Importação

| Evento                     | Descrição                                       |
|---------------------------|-------------------------------------------------|
| `LOG[Importação]`         | Informações sobre cada partnumber processado   |
| `LOG[Erro de Cálculo]`    | Mensagens sobre falha na margem ou preço       |
| `LOG[Validação]`          | Erros em datas ou formato de planilha          |
| `LOG[RegrasAplicadas]`    | Quais regras da view foram aplicadas e por quê |

Todos os logs devem conter:
- ID do usuário
- Timestamp
- Nome do arquivo
- Linha da planilha, se aplicável