using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_NOTA_FISCAL_STATUS_NOTA")]
    public class StatusNotaFiscal
    {
        [Key] [Column("CD_NOTA_FISCAL_STATUS_NOTA")] public int Codigo { get; set; }
        
        [Column("CD_NOTA_FISCAL")] public int IdNotaFiscal { get; set; }
        
        [Column("CD_STATUS_NOTA")] public int IdStatus { get; set; }
        
        [Column("DT_INCLUSAO")] public DateTime? DataInclusao { get; set; }
        
        [ForeignKey("IdStatus")]
        public virtual Status Status { get; set; }
    }
}
