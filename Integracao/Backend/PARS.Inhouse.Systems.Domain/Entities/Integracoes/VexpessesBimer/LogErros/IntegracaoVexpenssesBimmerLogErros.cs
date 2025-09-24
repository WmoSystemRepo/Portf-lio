using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PARS.Inhouse.Systems.Domain.Entities.Integracoes.VexpessesBimer.LogErros
{
    public class IntegracaoVexpenssesBimmerLogErros
    {
        [Key]
        public long Id { get; set; }

        public DateTime DataHoraUtc { get; set; }

        [MaxLength(256)]
        public string? Endpoint { get; set; }

        [MaxLength(16)]
        public string? HttpMethod { get; set; }

        [MaxLength(512)]
        public string? Path { get; set; }

        [MaxLength(2000)]
        public string? QueryString { get; set; }

        [MaxLength(256)]
        public string? UserName { get; set; }

        [MaxLength(64)]
        public string? ClientIp { get; set; }

        [MaxLength(128)]
        public string? TraceId { get; set; }

        [MaxLength(512)]
        public string? ExceptionType { get; set; }

        [MaxLength(2000)]
        public string? Message { get; set; }

        public string? StackTrace { get; set; }

        public string? InnerException { get; set; }

        public string? Payload { get; set; }
    }
}
