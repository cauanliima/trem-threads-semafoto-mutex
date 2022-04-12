using System;
using CHESF.COMPRAS.Domain.SGNF;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class StatusDTO
    {
        public int Codigo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataInclusao { get; set; }

        public static StatusDTO FromStatusNotaFiscal(StatusNotaFiscal? status)
        {
            if (status?.Status == null)
            {
                return null;
            }

            return new StatusDTO
            {
                Codigo = status.Status.Codigo,
                Descricao = status.Status.Descricao,
                DataInclusao = status.DataInclusao
            };
        }
    }
}