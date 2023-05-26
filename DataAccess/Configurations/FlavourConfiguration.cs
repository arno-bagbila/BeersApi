using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class FlavourConfiguration : IEntityTypeConfiguration<Flavour>
   {
      public void Configure(EntityTypeBuilder<Flavour> builder)
      {
         builder.ToTable("Flavour");

         builder.HasKey(f => f.Id);

         builder.Property(f => f.Id);

         builder.Property(f => f.UId)
            .IsRequired();

         builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(50);

         builder.Property(f => f.Description)
            .IsRequired()
            .HasMaxLength(3000);
      }
   }
}
