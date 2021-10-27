using System;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    public class Licitacao
    {
        public int? Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Numero { get; set; }
        public char CriterioJulgamento { get; set; }
        public DateTime DataEnvioInicial { get; set; }
        public DateTime DataEnvioFinal { get; set; }
        public DateTime AberturaPropostas { get; set; }
    }
}