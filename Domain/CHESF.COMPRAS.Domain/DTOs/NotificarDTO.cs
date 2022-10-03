using System.Collections.Generic;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class NotificarDTO
    {
        public string? Titulo { get; set; }
        public string Tipo { get; set; }
        public string Texto { get; set; }
        public string? Cnpj { get; set; }
        public Dictionary<string, string> Payload { get; set; }
        public Dictionary<string, string> Metadados { get; set; }
    }
}