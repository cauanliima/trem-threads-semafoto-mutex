using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    [Table("TB_LICITACAO", Schema = "dbo")]
    public class Licitacao
    {
        private string _modalidade;
        private string _status;
        [Key] [Column("NR_PRCS")] public long Codigo { get; set; }

        [Column("IC_MODALIDADE")]
        public string Modalidade
        {
            get => _modalidade.Trim();
            set => _modalidade = value;
        }

        [Column("DS_PRCS")] public string? Descricao { get; set; }

        [Column("IC_STATUS")]
        public string Status
        {
            get => _status.Trim();
            set => _status = value;
        }

        [Column("IC_CRITERIO")] public char? CriterioJulgamento { get; set; }

        [Column("DT_AQUISICAO_EDITAL")] public DateTime? AberturaPropostas { get; set; }

        [NotMapped] public DateTime? DataEnvioInicial { get; set; }
        [NotMapped] public DateTime? DataEnvioFinal { get; set; }

        public string Numero
        {
            get
            {
                var c = Codigo.ToString();
                if (c.Length <= 4) return c;
                var resultado = c.Length >= 10
                    ? $"{c.Substring(0, c.Length - 8)}.{c.Substring(c.Length - 8, 4)}.{c.Substring(c.Length - 4)}"
                    : $"{c.Substring(0, c.Length - 4)}/{c.Substring(c.Length - 4)}";
                return resultado;
            }
        }
    }
}