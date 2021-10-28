using System;
using System.Collections.Generic;
using CHESF.COMPRAS.Domain.E_Edital;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class LicitacaoDTO
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Numero { get; set; }
        public DateTime DataEnvioFinal { get; set; }
    }
}