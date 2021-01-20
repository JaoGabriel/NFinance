using Microsoft.EntityFrameworkCore;
using NFinance.Domain;
using NFinance.Domain.Interfaces.Repository;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(decimal))))
            {
                property.SetColumnType("DECIMAL");
                property.SetPrecision(38);
                property.SetDefaultValue(0);
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDadosContext).Assembly);
        }

        public async Task<int> Commit()
        {
            return await base.SaveChangesAsync();
        }
    }
}
