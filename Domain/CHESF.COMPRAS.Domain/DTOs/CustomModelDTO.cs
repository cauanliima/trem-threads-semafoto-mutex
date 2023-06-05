using System.Collections.Generic;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class CustomModelDTO
    {
        public string? Message { get; set; }
        public string? Environment { get; set; }
        public List<string?>? Destinatarios { get; set; }
    }
}