using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_FORNECEDOR", Schema = "SGNF")]
    public class Fornecedor 
    {
        [Key] [Column("CD_FORNECEDOR")] public long Codigo { get; set; }
        
        [Column("NR_CNPJ")] public long CNPJ { get; set; }
        
        [Column("NM_FANTASIA")] public string NomeFantasia { get; set; }
        
        [Column("NM_RAZAO_SOCIAL")] public string RazaoSocial { get; set; }

    }
}