namespace PARS.Inhouse.Systems.Shared.DTOs.Integracoes.VexpenssesBimer.Response.Bimer
{
    /// <summary>
    /// Representa o DTO de resposta com dados de pessoas (fornecedores ou clientes) 
    /// recebidos da integração com o sistema Bimer via VExpenses.
    /// </summary>
    public class PessoaResponseDto
    {
        /// <summary>
        /// Lista de objetos pessoa retornados pela consulta.
        /// </summary>
        public List<PessoaObjetoDto>? ListaObjetos { get; set; }

        /// <summary>
        /// Identificador principal da pessoa retornada.
        /// </summary>
        public string? Identificador { get; set; }

        /// <summary>
        /// Lista de erros encontrados durante o processamento, se houver.
        /// </summary>
        public List<string>? Erros { get; set; }
    }

    /// <summary>
    /// Representa os dados cadastrais de uma pessoa integrável (cliente/fornecedor).
    /// </summary>
    public class PessoaObjetoDto
    {
        /// <summary>
        /// Categorias associadas à pessoa.
        /// </summary>
        public List<CategoriaDto>? Categorias { get; set; }

        /// <summary>
        /// Data de cadastro da pessoa no sistema.
        /// </summary>
        public DateTime DataCadastro { get; set; }

        /// <summary>
        /// Lista de endereços associados. Pode ser uma estrutura customizada.
        /// </summary>
        public List<object>? Enderecos { get; set; }

        /// <summary>
        /// Identificador único da pessoa.
        /// </summary>
        public string? Identificador { get; set; }

        /// <summary>
        /// Indica se os dados da pessoa têm acesso restrito.
        /// </summary>
        public bool InformacoesRestritas { get; set; }

        /// <summary>
        /// Indica se a pessoa possui alguma inadimplência registrada.
        /// </summary>
        public bool PossuiCaracteristicaInadimplencia { get; set; }

        /// <summary>
        /// Tipo da pessoa (1 = física, 2 = jurídica, etc.).
        /// </summary>
        public int TipoPessoa { get; set; }

        /// <summary>
        /// Tipo de cliente no setor de telecomunicação, se aplicável.
        /// </summary>
        public int TipoClienteTelecomunicacao { get; set; }

        /// <summary>
        /// Identificador do CNAE da empresa.
        /// </summary>
        public string? IdentificadorCNAE { get; set; }

        /// <summary>
        /// Ramo de atividade da pessoa jurídica.
        /// </summary>
        public string? RamoAtividade { get; set; }

        /// <summary>
        /// Identificador da situação cadastral atual da pessoa.
        /// </summary>
        public string? IdentificadorSituacaoCadastralPessoa { get; set; }

        /// <summary>
        /// Código interno utilizado para identificação da pessoa.
        /// </summary>
        public string? Codigo { get; set; }

        /// <summary>
        /// Número do CPF ou CNPJ em formato numérico.
        /// </summary>
        public long CpfCnpj { get; set; }

        /// <summary>
        /// CPF ou CNPJ formatado com máscara.
        /// </summary>
        public string? CpfCnpjCompleto { get; set; }

        /// <summary>
        /// Nome completo da pessoa.
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Nome curto ou fantasia da empresa/pessoa.
        /// </summary>
        public string? NomeCurto { get; set; }

        /// <summary>
        /// Data de início das atividades da empresa/pessoa.
        /// </summary>
        public DateTime DataInicioAtividades { get; set; }

        /// <summary>
        /// Indica se a entidade está vinculada à Administração Pública Federal.
        /// </summary>
        public bool EntidadeAdministracaoPublicaFederal { get; set; }
    }

    /// <summary>
    /// Representa uma categoria associada à pessoa no Bimer.
    /// </summary>
    public class CategoriaDto
    {
        /// <summary>
        /// Indica se a categoria está ativa.
        /// </summary>
        public bool Ativo { get; set; }

        /// <summary>
        /// Código externo da categoria.
        /// </summary>
        public string? CodigoExterno { get; set; }

        /// <summary>
        /// Identificador interno da categoria.
        /// </summary>
        public string? Identificador { get; set; }

        /// <summary>
        /// Nome descritivo da categoria.
        /// </summary>
        public string? Nome { get; set; }
    }
}
