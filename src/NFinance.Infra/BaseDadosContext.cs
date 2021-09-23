using System;
using System.Linq;
using NFinance.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NFinance.Domain.Identidade;

namespace NFinance.Infra
{
    public class BaseDadosContext : IdentityDbContext<Usuario,Role,Guid>
    {
        public BaseDadosContext(DbContextOptions<BaseDadosContext> options) : base(options){}

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Gasto> Gasto { get; set; }
        public DbSet<Investimento> Investimento { get; set; }
        public DbSet<Resgate> Resgate { get; set; }
        public DbSet<Ganho> Ganho { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
                
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(decimal))))
            {
                property.SetColumnType("DECIMAL");
                property.SetPrecision(38);
                property.SetDefaultValue(0);
            }
            
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseDadosContext).Assembly);
        }
    }
}
