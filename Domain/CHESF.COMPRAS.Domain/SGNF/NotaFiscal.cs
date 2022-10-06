using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CHESF.COMPRAS.Domain.DTOs;
using Newtonsoft.Json;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_NOTA_FISCAL")]
    public class NotaFiscal
    {
        [Key] [Column("CD_NOTA_FISCAL")] public int Codigo { get; set; }
        
        [Column("NR_NOTA_FISCAL")] public string Numero { get; set; }
        
        [Column("CD_FORNECEDOR")] public int Fornecedor { get; set; }
        
        [Column("CD_CONTRATO")] public int? IdContrato { get; set; }
        
        [Column("VL_NOTA")] public double? Valor { get; set; }
        
        [Column("NR_MES")] public int? Mes { get; set; }
        
        [Column("NR_ANO")] public int? Ano { get; set; }
        
        [Column("CD_ADMINISTRADOR")]
        public int? IdAdministrador { get; set; }
        
        [Column("DT_EMISSAO")] public DateTime? DataEmissao { get; set; }
        
        [Column("DT_INCLUSAO")] public DateTime? DataInclusao { get; set; }

        [JsonIgnore]
        [Column("CD_NOTA_FISCAL_STATUS_NOTA")]
        public int? IdStatus { get; set; }
        
        [JsonIgnore]
        [ForeignKey("IdContrato")]
        public virtual Contrato? Contrato { get; set; }
        
        [JsonIgnore]
        [ForeignKey("IdStatus")]
        public virtual StatusNotaFiscal? StatusNotaFiscal { get; set; }
        
        [JsonIgnore]
        public virtual List<StatusNotaFiscal> HistoricoNotaFiscal { get; set; }

        public virtual StatusDTO? Status => StatusDTO.FromStatusNotaFiscal(StatusNotaFiscal);
        
        [NotMapped] public DateTime? DataPagamento { get; set; }
    }
}