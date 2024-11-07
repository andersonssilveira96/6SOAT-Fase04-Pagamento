using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infra.Data.Config
{
    public class PagamentoConfig : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                    .UseIdentityColumn();

            builder.Property(x => x.Status).IsRequired();
        }
    }
    
}
