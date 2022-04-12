using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_CONTRATO")]
    public class Contrato
    {
     
        [Key] [Column("CD_CONTRATO")] public int Codigo { get; set; }
        
        [Column("NR_CONTRATO")] public string Numero { get; set; }
        
        [Column("VL_CONTRATO")] public double Valor { get; set; }
        
        [Column("DS_OBJETO")] public string Objeto { get; set; }
        
        [Column("NR_CONTRATO_SAP")] public string NumeroContratoSAP { get; set; }
        
        [Column("IC_ATIVO")] public bool Ativo { get; set; }
        
        [Column("DT_INCLUSAO")] public DateTime?  DataInclusao { get; set; }
        
        [JsonIgnore]
        public virtual ICollection<ContratoFornecedor> ContratoFornecedores { get; set; }
    }
}