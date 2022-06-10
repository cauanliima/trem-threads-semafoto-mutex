using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_CONTRATO_FORNECEDOR")]
    public class ContratoFornecedor
    {
        [Key] [Column("CD_CONTRATO_FORNECEDOR")] public int Codigo { get; set; }
        
        [Column("CD_CONTRATO")] public int IdContrato { get; set; }
        
        [Column("CD_FORNECEDOR")] public int IdFornecedor { get; set; }
        
        [Column("DT_INICIO_SERVICO")] public DateTime? DataInicio { get; set; }
        
        [Column("DT_FIM_SERVICO")] public DateTime? DataFim { get; set; }
        
        [Column("DT_INICIO_SERVICO")] public DateTime? DataInicioServico { get; set; }
        
        [Column("DT_FIM_SERVICO")] public DateTime? DataFimServico { get; set; }
        
        [Column("NR_DIAS_PAGAMENTO")] public int? NumeroDiasPagamentos { get; set; }
        
        [Column("VL_PAGO")] public double? ValorPago { get; set; }
        
        [Column("VL_PREVISTO")] public double? ValorPrevisto { get; set; }
        
        [Column("VL_REALIZADO")] public double? ValorRealizado { get; set; }
        
        [ForeignKey("IdContrato")] public virtual Contrato Contrato { get; set; }
        
        [ForeignKey("IdFornecedor")] public virtual Fornecedor Fornecedor { get; set; }
        
    }
}