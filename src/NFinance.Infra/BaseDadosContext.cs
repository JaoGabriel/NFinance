using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System.Threading.Tasks;

namespace NFinance.Infra
{
    public class BaseDadosContext : DbContext, IUnitOfWork
    {
        public BaseDadosContext(DbContextOptions<BaseDadosContext> options) : base(options){}

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Gastos> Gastos { get; set; }
        public DbSet<PainelDeControle> PainelDeControle { get; set; }
        public DbSet<Investimentos> Investimentos { get; set; }
        public DbSet<Resgate> Resgate { get; set; }

        public async Task<bool> Commit()
        {
            return await base.SaveChangesAsync() > 0;
        }
    }
}
