using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_STATUS_NOTA", Schema = "SGNF")]
    public class Status
    {
        [Key] [Column("CD_STATUS")] public long Codigo { get; set; }
        
        [Column("DS_STATUS_NOTA")] public string Descricao { get; set; }
        
        [Column("IC_ATIVO")] public bool Ativo { get; set; }
    }
}