using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_STATUS_NOTA")]
    public class Status
    {
        [Key] [Column("CD_STATUS_NOTA")] public int Codigo { get; set; }
        
        [Column("DS_STATUS_NOTA")] public string Descricao { get; set; }
        
        [JsonIgnore]
        [Column("IC_ATIVO")] public bool Ativo { get; set; }
    }
}