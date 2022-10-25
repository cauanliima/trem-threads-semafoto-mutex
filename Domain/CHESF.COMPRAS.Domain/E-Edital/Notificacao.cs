using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_NOTIFICACAO", Schema = "dbo")]
    public class Notificacao
    {
        public Notificacao()
        {
            TentativasEntregas = new List<NotificacaoDispositivo>();
        }
       
        [Key] [Column("ID_NOTIFICACAO")] public int Id { get; set; }
        [Column("DS_TITULO")] public string? Titulo { get; set; }
        [Column("TP_NOTIFICACAO")] public string Tipo { get; set; }
        [Column("DS_TEXTO")] public string Texto { get; set; }
        
        [Column("NR_CNPJ")] public string? Cnpj { get; set; }
        
        [JsonIgnore]
        [Column("VL_PAYLOAD")] public string? PayloadStr { get; set; }
        
        [JsonIgnore]
        [Column("VL_METADADOS")] public string? MetadadosStr { get; set; }
        [Column("DT_CADASTRO")] public DateTime DataCadastro { get; set; }

        [JsonIgnore]
        public List<NotificacaoDispositivo> TentativasEntregas { get; set; }
        
        [NotMapped]
        public Dictionary<string, string> Payload
        {
            get
            {
                if (PayloadStr == null)
                {
                    return new Dictionary<string, string>();
                }

                var concrete = JsonSerializer.Deserialize<Dictionary<string, string>>(PayloadStr);

                return concrete ?? new Dictionary<string, string>();
            }
            set => PayloadStr = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public Dictionary<string, string> Metadados
        {
            get
            {
                if (MetadadosStr == null)
                {
                    return new Dictionary<string, string>();
                }

                var concrete = JsonSerializer.Deserialize<Dictionary<string, string>>(MetadadosStr);

                return concrete ?? new Dictionary<string, string>();
            }
            set => MetadadosStr = JsonSerializer.Serialize(value);
        }
    }
}