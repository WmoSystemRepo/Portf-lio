# 📄 Documentação do Componente: IntegraçãoAdobeHub

## 🔍 Visão Geral

* **Nome do componente:** IntegraçãoAdobeHub
* **Caminho:** `integracoes/AdobeHub/importacao/csv-execel/importar-csv-execel.component.ts`
* **Objetivo:** Permitir a importação de arquivos Excel/CSV, gerar templates e visualizar dados antes da importação final no sistema.

---

## ⚙️ Funcionalidades Gerais

* Upload de arquivos `.csv`, `.xls`, `.xlsx` por botão ou arraste.
* Seleção de tipo de template via botões toggle.
* Inferência automática do template pelo nome do arquivo.
* Visualização da planilha como matriz de dados.
* Confirmação visual de linha de cabeçalho.
* Mapeamento de colunas do Excel com nome do cabeçalho.
* Persistência de template no sistema.
* Listagem dos templates e planilhas importadas.
* Filtros e ordenações nas tabelas.
* Opções de editar, excluir, visualizar e exportar.

---

## 🧩 Componentes Internos e Funcionalidades

### 1. **Upload de Arquivo (Drag & Drop)**

#### Funcionalidades:

* Aceita `.csv`, `.xls`, `.xlsx`
* Upload via botão ou área de "arraste"
* Exibe nome e tamanho do arquivo após seleção
* Mostra ícone do tipo de arquivo (ex: Excel)
* Botão de exclusão do arquivo aparece ao lado

#### Regras:

* Extensões inválidas são rejeitadas
* A seleção de template deve preceder a visualização dos dados
* Botão de "Pré-visualizar Dados" só é habilitado com template e arquivo

---

### 2. **Seleção de Template**

#### Funcionalidades:

* Botões estilo toggle com nome dos templates disponíveis (VIP e 3YR)
* Destaca visualmente o botão selecionado
* Detecta automaticamente o template a partir do nome do arquivo (ex: contém `vipmpe` → VIP\_MP\_Educacional)

#### Regras:

* Apenas um botão ativo por vez
* A seleção é obrigatória para continuar
* Templates são identificáveis por sufixo padrão no nome do arquivo

---

### 3. **Visualização CSV / Excel**

#### Funcionalidades:

* Apresenta planilha lida como tabela com colunas nomeadas
* Linha de cabeçalho destacada visualmente (fundo preto)
* Botões para voltar, baixar Excel, salvar no Mongo
* Scroll horizontal para planilhas largas

#### Regras:

* A visualização só é acessível com template e arquivo válidos
* Nome do template aparece na parte superior
* Exibe até células vazias para respeitar estrutura do Excel original

---

### 4. **Lista de Excel Importados**

#### Funcionalidades:

* Tabela com colunas "Tipo Template" e "Nome Arquivo Base"
* Filtros individuais por coluna
* Ações disponíveis por item:

  * Editar (ícone verde)
  * Excluir (ícone vermelho)
  * Exportar (ícone laranja)
  * Visualizar (ícone azul)
* Seleção em massa via checkboxes
* Botão para exportar múltiplos Excel selecionados

#### Regras:

* Checkbox de seleção afeta o botão de exportação em lote
* Exportação gera `.xlsx` com nome padrão `exportacao_excel_{data}`
* Botão "Limpar" remove filtros ativos

---

## 🧠 Fluxo de Execução (Resumo)

```text
1. Usuário acessa componente de importação
2. Seleciona ou arrasta arquivo válido (.csv/.xlsx)
3. Template é detectado automaticamente ou selecionado manualmente
4. Visualização da planilha e marcação da linha de cabeçalho
5. Confirmação e transição para tela de mapeamento
6. Usuário ajusta colunas, nomes de cabeçalho e salva template
7. Template passa a aparecer na lista geral
8. Planilhas importadas ficam disponíveis para exportação e análise posterior
```

---

## 📂 Arquivos Relacionados

* `importar-csv-execel.component.ts`
* `importar-csv-execel.component.html`
* `importar-csv-execel.component.css`

---

## 📦 Dependências e Tecnologias

* `XLSX` – leitura de planilhas Excel/CSV
* `SweetAlert2` – mensagens e feedbacks visuais
* `Router` – navegação entre componentes Angular
* `FormsModule` – controle de formulários e campos
* `TipoTemplateEnum` – enumeração de tipos reconhecidos

---

## 💡 Melhorias Futuras Sugeridas

* Validação de tamanho de arquivo e número de colunas
* Suporte a múltiplos arquivos em lote
* Feedback visual mais claro para erros e validações
* Internacionalização (i18n) para multilinguagem
* Permitir salvar configurações prévias por usuário (ex: template favorito)
* Indicar visualmente progresso durante leitura de arquivos grandes
* Otimizar feedback para usuários com acessibilidade reduzida
