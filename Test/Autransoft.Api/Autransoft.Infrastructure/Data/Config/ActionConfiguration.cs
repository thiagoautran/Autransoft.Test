using Autransoft.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Autransoft.Infrastructure.Data.Config
{
    public class EscortConfiguration : IEntityTypeConfiguration<ActionEntity>
    {
        public void Configure(EntityTypeBuilder<ActionEntity> builder)
        {
            builder.ToTable("action");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id")
                .IsRequired();

            builder.Property(x => x.CompanyId)
                .HasColumnName("company_id");

            builder.Property(x => x.CompanyName)
                .HasColumnName("company_name");

            builder.Property(x => x.Ticker)
                .HasColumnName("ticker");

            builder.Property(x => x.LastUpdate)
                .HasColumnName("last_update");
        }
    }
}