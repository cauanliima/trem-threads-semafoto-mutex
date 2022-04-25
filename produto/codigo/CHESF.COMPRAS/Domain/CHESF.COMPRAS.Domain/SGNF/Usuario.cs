using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CHESF.COMPRAS.Domain.SGNF
{
    [Table("TB_USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("CD_USUARIO")]
        public int Codigo { get; set; }
        
        [Column("NM_USUARIO")]
        public string Nome { get; set; }
        
        [Column("NR_MATRICULA")]
        public string Matricula { get; set; }
        
        [Column("NM_LOGIN")]
        public string Login { get; set; }
        
        [Column("EN_EMAIL")]
        public string Email { get; set; }
    }
}