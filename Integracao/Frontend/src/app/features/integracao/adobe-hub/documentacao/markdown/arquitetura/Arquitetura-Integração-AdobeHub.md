# 🧭 Arquitetura do Módulo de Importação – Angular 17

Este documento descreve a organização dos componentes e fluxos do módulo de **importação de arquivos Excel/CSV** e **gerenciamento de templates (modelos)** dentro do sistema **AdobeHub**, desenvolvido em **Angular 17**.

---

## 📁 Estrutura Geral de Pastas

integracoes/
└── AdobeHub/
└── importacao/
├── csv-excel/
│ ├── enviar-arquivo/
│ │ └── importar-excel.component.*
│ └── visualizar-importacao/
│ └── ImportExcelPreviewComponent.*
├── historico-importacoes/
│ ├── import-history.component.*
│ └── import-history-details.component.*
├── inicio/
│ └── inicio.component.*
└── modelos/
├── criar-modelo/
│ └── criar-modelo.component.*
├── enviar-modelo/
│ └── enviar-modelo.component.*
└── visualizar-modelo/
└── visualizar-modelo.component.*


---

## 📦 Descrição por Módulo

### 📂 `csv-excel/`

Subdividido em:

#### 📁 `enviar-arquivo/`
- **Componente:** `importar-excel.component.ts`
- **Responsável por:**
  - Upload de arquivos `.csv`, `.xls`, `.xlsx`
  - Processamento via `xlsx` para JSON
  - Redirecionamento para a tela de visualização da planilha

#### 📁 `visualizar-importacao/`
- **Componente:** `ImportExcelPreviewComponent.ts`
- **Responsável por:**
  - Apresentar os dados carregados do arquivo
  - Pré-visualização antes da importação definitiva (validação e preview de dados)

---

### 📂 `historico-importacoes/`

- **Componentes:**
  - `import-history.component.ts`
  - `import-history-details.component.ts`

- **Responsável por:**
  - Listar os históricos de importações realizadas
  - Exibir detalhes completos de cada execução (data, arquivo, erros etc.)

---

### 📂 `inicio/`

- **Componente:** `inicio.component.ts`
- **Responsável por:**
  - Tela inicial do módulo de importação
  - Acesso aos caminhos:
    - Importar Arquivo
    - Criar Novo Modelo
    - Histórico de Importações

---

## 🧩 Módulo de Modelos (`modelos/`)

Subdividido por função:

### 📁 `criar-modelo/`
- **Componente:** `criar-modelo.component.ts`
- **Responsável por:**
  - Mapeamento manual de colunas da planilha para letras (ex: A, B, C...)
  - Edição e visualização dos campos do template
  - Botões: salvar, cancelar, excluir campos

### 📁 `enviar-modelo/`
- **Componente:** `enviar-modelo.component.ts`
- **Responsável por:**
  - Upload de um arquivo base para gerar um novo modelo
  - Comportamento similar ao importar, mas voltado à criação de template

### 📁 `visualizar-modelo/`
- **Componente:** `visualizar-modelo.component.ts`
- **Responsável por:**
  - Pré-visualização da planilha
  - Permite ao usuário selecionar a linha e coluna que representam o cabeçalho
  - Validação de células mescladas ou vazias
  - Geração de um `TemplateModel` completo para ser enviado à etapa de mapeamento

---

## 🔁 Fluxo do Usuário

```text
1. Acessa a tela de início: /inicio
2. Escolhe uma das opções:
   a) Importar Arquivo → /csv-excel/enviar-arquivo
   b) Criar Modelo → /modelos/enviar-modelo
   c) Visualizar Histórico → /historico-importacoes
3. Após o upload:
   a) Visualiza o conteúdo da planilha → /csv-excel/visualizar-importacao
   b) Ou seleciona a linha de cabeçalho → /modelos/visualizar-modelo
4. Após selecionar o cabeçalho:
   a) Realiza mapeamento de campos → /modelos/criar-modelo
5. Salva template e reutiliza em futuras importações
