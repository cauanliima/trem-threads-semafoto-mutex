using System;
using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.QueryParams
{
    public class LicitacaoFiltroQueryParams : ListaQueryParams
    {
        public string texto { get; set; }
        public string situacao { get; set; }
        public string modalidade { get; set; }

        public DateTime? inicio { get; set; }
        public DateTime? fim { get; set; }
    }
}