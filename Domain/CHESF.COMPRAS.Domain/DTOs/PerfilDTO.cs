using Newtonsoft.Json;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class PerfilDTO
    {
        [JsonProperty("nmPerfil")]
        public string Nome { get; set; }
    }
}