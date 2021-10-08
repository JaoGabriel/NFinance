using Microsoft.AspNetCore.Identity;
using NFinance.Domain.Identidade;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NFinance.Infra.Exceptions;
using NFinance.Infra.Token;
using NFinance.Domain.Interfaces.Repository;

namespace NFinance.Infra.Repository
{
    [ExcludeFromCodeCoverage]
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UsuarioRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Usuario> CadastrarUsuario(string email, string senha)
        {
            var user = new Usuario() { Email = email };
            var resultadoCadastro = await _userManager.CreateAsync(user, senha);

            if (!resultadoCadastro.Succeeded) throw new InfraException("Ocorreu um erro, tente novamente mais tarde.");

            return user;
        }

        public async Task<Usuario> Conectar(string email, string senha)
        {
            var loginResponse = await _signInManager.PasswordSignInAsync(email, senha, false, true);

            if (!loginResponse.Succeeded)
                throw new InfraException("Usuario ou senha inválido, tente novamente");
            
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task Desconectar()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
