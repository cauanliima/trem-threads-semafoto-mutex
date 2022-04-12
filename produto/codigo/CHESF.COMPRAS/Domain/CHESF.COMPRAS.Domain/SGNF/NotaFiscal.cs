using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_NOTA_FISCAL", Schema = "SGNF")]
    public class NotaFiscal
    {
        [Key] [Column("CD_NOTA_FISCAL")] public long Codigo { get; set; }
        
        [Column("NR_NOTA_FISCAL")] public long Numero { get; set; }
        
        [Column("CD_FORNECEDOR")] public long Fornecedor { get; set; }
        
        [Column("CD_CONTRATO")] public long Contrato { get; set; }
        
        [Column("VL_NOTA")] public double Valor { get; set; }
        
        [Column("NR_MES")] public int Mes { get; set; }
        
        [Column("NR_ANO")] public int Ano { get; set; }
        
        [Column("DT_EMISSAO")] public DateTime? DataEmissao { get; set; }
    }
}