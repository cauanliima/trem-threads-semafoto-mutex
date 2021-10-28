using System;

namespace CHESF.COMPRAS.Domain.E_Edital
{
    public class Anexo
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Extensao { get; set; }
        public DateTime DataCriacao { get; set; }
        public int CodigoLicitacao { get; set; }
        public string Link { get; set; }
    }
}