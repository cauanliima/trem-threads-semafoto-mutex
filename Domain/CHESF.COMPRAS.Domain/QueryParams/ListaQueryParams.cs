using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.QueryParams
{
    public class ListaQueryParams
    {
        [Required] public int Total { get; set; }
        [Required] public int Pagina { get; set; }
    }
}