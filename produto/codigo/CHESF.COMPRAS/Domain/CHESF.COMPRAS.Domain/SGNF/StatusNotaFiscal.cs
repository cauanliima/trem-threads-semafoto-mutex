using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_NOTA_FISCAL_STATUS_NOTA", Schema = "SGNF")]
    public class StatusNotaFiscal
    {
        [Key] [Column("CD_NOTA_FISCAL_STATUS_NOTA")] public long Codigo { get; set; }
        
        [Column("CD_NOTA_FISCAL")] public long NotaFiscal { get; set; }
        
        [Column("CD_STATUS_NOTA")] public long Status { get; set; }
        
        [Column("DT_INCLUSAO")] public DateTime? DataInclusao { get; set; }
        
        
    }
}
