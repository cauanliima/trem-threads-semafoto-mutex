using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.QueryParams
{
    public abstract class ListaQueryParams
    {
        [Required] public int total { get; set; }
        [Required] public int pagina { get; set; }
    }
}