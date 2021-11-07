using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_LICITACAO", Schema = "dbo")]
    public class Licitacao
    {
        [Key] [Column("NR_PRCS")] public long Codigo { get; set; }

        [Column("IC_MODALIDADE")] public string Modalidade { get; set; }
        [Column("DS_PRCS")] public string? Descricao { get; set; }
        [Column("IC_STATUS")] public string Status { get; set; }

        [Column("IC_CRITERIO")] public char? CriterioJulgamento { get; set; }

        [Column("DT_AQUISICAO_EDITAL")] public DateTime? AberturaPropostas { get; set; }
        
        [NotMapped] public DateTime? DataEnvioInicial { get; set; }
        [NotMapped] public DateTime? DataEnvioFinal { get; set; }

        public string Numero => Codigo.ToString();
    }
}