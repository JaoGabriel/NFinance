using NFinance.Domain.Identidade;
using System.Threading.Tasks;

namespace NFinance.Domain.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario> Conectar(string email, string senha);

        Task Desconectar();

        Task<Usuario> CadastrarUsuario(string email, string senha);
    }
}