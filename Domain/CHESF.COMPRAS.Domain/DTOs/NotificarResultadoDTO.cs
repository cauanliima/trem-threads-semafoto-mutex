using System.Collections.Generic;

namespace CHESF.COMPRAS.Domain.DTOs
{
    public class NotificarResultadoDTO
    {
        public NotificarResultadoDTO()
        {
            DispositivosComFalha = new List<NotificarResultadoFalhaDispositivoDTO>();
        }
       
        public int SucessoContagem { get; set; }
        public int FalhaContagem { get; set; }

        public List<NotificarResultadoFalhaDispositivoDTO> DispositivosComFalha { get; set; }
    }
}