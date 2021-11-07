using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;

namespace CHESF.COMPRAS.IService
{
    public interface IAnexoService
    {
        Task<IEnumerable<Anexo>> TodosDaLicitacao(long id);
        Task<ArquivoDTO> Baixar(int id);
    }
}