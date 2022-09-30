using System.Collections.Generic;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class DispositivoDTO
    {
        public string FirebaseId { get; set; }
        public string FcmToken { get; set; }
        public string? Cnpj { get; set; }
        public Dictionary<string, string> Metadados { get; set; }
    }
}