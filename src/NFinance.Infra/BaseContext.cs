using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces;
using System.Threading.Tasks;

namespace NFinance.Infra
{
    public class BaseContext : DbContext, IUnitOfWork
    {
        public BaseContext(DbContextOptions<BaseContext> options) : base(options){}

        DbSet<Cliente> Cliente { get; set; }
        DbSet<Gastos> Gastos { get; set; }
        DbSet<PainelDeControle> PainelDeControle { get; set; }
        DbSet<Investimentos> Investimentos { get; set; }
        DbSet<Resgate> Resgate { get; set; }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
