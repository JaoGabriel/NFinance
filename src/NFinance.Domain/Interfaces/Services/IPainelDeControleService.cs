using System.Collections.Generic;
using System.Threading.Tasks;

namespace NFinance.Domain.Interfaces.Services
{
    public interface IPainelDeControleService
    {
        Task<PainelDeControle> GerarSaldoMensal();
        Task<PainelDeControle> GerarSaldoAnual();
        Task<PainelDeControle> AdicionarSaldoCarteira();
        Task<PainelDeControle> ResgatarSaldoCarteira();
        Task<PainelDeControle> AtualizarSaldoCarteira();
        Task<PainelDeControle> ConsultarValorNaCarteira();
        Task<List<PainelDeControle>> ListarGastoMensal();
        Task<List<PainelDeControle>> ListarGastoAnual();
        Task<List<PainelDeControle>> ListarInvestimentoMensal();
        Task<List<PainelDeControle>> ListarInvestimentoAnual();
        Task<PainelDeControle> ConsultarRecebidoAnual();
    }
}
