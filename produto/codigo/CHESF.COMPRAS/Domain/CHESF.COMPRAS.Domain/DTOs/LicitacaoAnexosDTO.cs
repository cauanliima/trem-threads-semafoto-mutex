using System.Collections.Generic;
using CHESF.COMPRAS.Domain.E_Edital;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class LicitacaoAnexosDTO
    {
        public Licitacao Licitacao { get; set; }
        public List<Anexo> Anexos { get; set; }
    }
}