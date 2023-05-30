using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_FORNECEDOR")]
    public class Fornecedor 
    {
        [Key] [Column("CD_FORNECEDOR")] public int Codigo { get; set; }
        
        [Column("NR_CNPJ")] public long CNPJ { get; set; }
        
        [Column("NM_FANTASIA")] public string NomeFantasia { get; set; }
        
        [Column("NM_RAZAO_SOCIAL")] public string RazaoSocial { get; set; }
        
        [Column("EN_EMAIL")] public string? Email { get; set; }

    }
}