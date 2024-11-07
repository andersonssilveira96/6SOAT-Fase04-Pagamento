using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infra.Data.Context
{
    public sealed class TechChallengeContext : DbContext
    {         
        public TechChallengeContext(DbContextOptions<TechChallengeContext> options)
            : base(options)
        {
        }

        public DbSet<Pagamento> Pagamento { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
         
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());         
        }
    }
}
