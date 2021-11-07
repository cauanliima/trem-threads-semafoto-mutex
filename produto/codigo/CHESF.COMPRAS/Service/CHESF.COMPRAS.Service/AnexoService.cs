using System.Collections.Generic;
using System.Threading.Tasks;
using CHESF.COMPRAS.Domain.DTOs;
using CHESF.COMPRAS.Domain.E_Edital;
using CHESF.COMPRAS.IRepository;
using CHESF.COMPRAS.IService;

namespace CHESF.COMPRAS.Service
{
    public class AnexoService : IAnexoService
    {
        private readonly IAnexoRepository _repository;

        public AnexoService(IAnexoRepository anexoRepository)
        {
            _repository = anexoRepository;
        }

        public async Task<IEnumerable<Anexo>> TodosDaLicitacao(long id)
        {
            return _repository.TodasDaLicitacao(id);
        }

        public async Task<ArquivoDTO> Baixar(int id)
        {
            var anexo = await _repository.FirstOrDefault(arquivo => arquivo.Codigo == id);
            return new ArquivoDTO()
            {
                nome = anexo.Nome,
                arquivo = anexo.Arquivo
            };
        }
    }
}