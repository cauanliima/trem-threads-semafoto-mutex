using System.Collections.Generic;
using Newtonsoft.Json;

namespace CHESF.COMPRAS.Domain.APP
{
    public class NotificationRequest
    {
        public string Texto { get; set; }
        public string NumeroLicitacao { get; set; }
        public string CodigoLicitacao { get; set; }
        public string Action { get; set; }
        public bool Silent { get; set; }
    }
}