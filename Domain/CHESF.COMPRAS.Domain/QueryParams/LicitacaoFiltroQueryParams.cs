using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CHESF.COMPRAS.Domain.QueryParams
{
    public class LicitacaoFiltroQueryParams : ListaQueryParams
    {
        public string? texto { get; set; }
        public List<string>? situacoes { get; set; }
        public List<string>? modalidades { get; set; }

        public DateTime? inicio { get; set; }
        public DateTime? fim { get; set; }
    }
}