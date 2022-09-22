using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class ArquivoDTO
    {
        [Required] public byte[] arquivo { get; set; }
        [Required] public string nome { get; set; }
    }
}