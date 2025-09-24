---

# Documentação Técnica - Backend | Sistema PARS Inhouse Systems

> Desenvolvido por: [Wenklesley Mendes de Oliveira](https://www.linkedin.com/in/wenklesley-mendes-oliveira/)

---

## ✨ Visão Geral

O sistema **PARS Inhouse Systems** é uma plataforma robusta de integrações entre diversos sistemas corporativos como VExpenses, Bimer, Adobe Hub, Bacen e SPP. A solução visa garantir rastreabilidade, governança de dados, logging de erros e reprocessamento de falhas, dentro de uma arquitetura moderna baseada em conceitos de Clean Architecture e DDD (Domain-Driven Design).

---

## 🌐 Tecnologias Utilizadas

* **.NET 7 / .NET 6**
* **Entity Framework Core (SQL Server)**
* **MongoDB Atlas**
* **Swagger/OpenAPI**
* **Hangfire** (jobs recorrentes e agendamentos)
* **JWT Bearer Authentication**
* **AutoMapper**
* **Brevo SMTP** (Envio de emails)
* **Docker / Docker Compose** (em ambientes preparados)
* **Microsoft Identity** (controle de usuários e permissões)

---

## 🔧 Arquitetura

* **DDD (Domain-Driven Design)**

  * Entidades, Repositórios, Aggregates, Services, Application Services.

* **Camadas**:

  * **Domain**: Entidades, enums, regras de negócio.
  * **Application**: Interfaces de serviço e serviços de aplicação.
  * **Infrastructure**: Repositórios e APIs externas.
  * **API (Presentation Layer)**: Controllers e Middlewares.

* **Padrões de Projeto**:

  * **Repository Pattern**
  * **Service Layer**
  * **Unit of Work** (implícito com EF Core)

* **Princípios SOLID**:

  * Amplamente aplicados na separação de responsabilidades.

---

## ⚖️ Integrações Realizadas

### 1. **VExpenses <> Bimer**

* Envio de relatórios e títulos a pagar do VExpenses para o ERP Bimer.
* Filtros por status.
* Exclusão de pendências com logging (MongoDB).
* Dashboard com KPIs mensais e totais.
* Logging estruturado em SQL Server e MongoDB.

### 2. **SPP <> Bimer (Invoices)**

* Monitoramento de invoices integradas entre SPP e Bimer.
* APIs de reprocessamento manual e em massa.
* De/Para de mensagens para erro customizado.
* Autenticação integrada com token JWT e log centralizado.

### 3. **Adobe Hub**

* Importação dinâmica de planilhas (Excel) com templates flexíveis.
* Template armazenado em MongoDB.
* DTOs descritivos e extensíveis.

### 4. **BACEN (Banco Central)**

* APIs para consulta de cotação de dólar e moedas.
* Logging de erro robusto via MongoDB.
* Job recorrente com Hangfire para atualização diária (agendado às 19:30).

---

## 📊 Dashboard e Observabilidade

* KPIs em tempo real:

  * Total de pendências, erros, sucessos, reabertos, reprovados e pagos
  * Pizza de erros por tipo
  * Pendências por status
  * Pendências excluídas por usuário
  * Integração com MongoDB para exclusão e histórico

---

## 🚀 DevOps / Executáveis

* **Hangfire Dashboard**: para monitoramento dos jobs
* **Swagger UI**: com agrupamento por controller e grupo de integração
* **Migration Runner**: suporte a execução de `--migrate` via linha de comando
* **Health Check de conexão** com SQL Server ao iniciar

---

## ⚖️ Segurança

* JWT com configuração completa de:

  * Issuer
  * Audience
  * Subject
* Validação de Claims por controller (User.Identity.Name)
* Regras de permissão por endpoint via `ApplicationUser`

---

## 🧼 Boas Práticas Aplicadas

* DTOs com descrição completa e intellisense XML
* Logging com TraceId, IP, username, stack trace e payloads
* Segregação clara entre responsabilidades (SRP)
* Uso de async/await com `CancellationToken`
* Tratamento de erros com `OperationCanceledException`
* Controllers organizados por grupos Swagger (ApiExplorerSettings)

---

## 📚 Portfolio (Pontos fortes)

* Integrações complexas e bem documentadas
* Logging distribuído com SQL e Mongo
* Uso de Hangfire para automatização
* Reprocessamento completo de dados com auditoria
* Controle fino de permissões de usuários (pode ler, escrever, configurar...)
* APIs RESTful responsivas e seguras
* Uso inteligente de Mongo para logs, dashboards e templates dinâmicos
* Design orientado a integrações escaláveis (clean boundaries)

---

## 🔐 Banco de Dados

### SQL Server

* Entity Framework Core
* Migrations automáticas com controle de ambiente
* Logging em tabelas dedicadas por integração

### MongoDB

* Templates do Adobe
* Logs de exclusão
* Logs de falha VExpenses e Bimer

---

## 🔍 Próximos passos sugeridos

* Implementar testes unitários com xUnit e Moq
* Cobertura de testes em camadas de serviço
* Metrics com Prometheus + Grafana
* CI/CD com GitHub Actions ou Azure DevOps

---

> Para dúvidas técnicas ou oportunidades profissionais:
> [LinkedIn: Wenklesley Mendes de Oliveira](https://www.linkedin.com/in/wenklesley-mendes-oliveira/)
