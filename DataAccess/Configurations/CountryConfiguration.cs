using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class CountryConfiguration : IEntityTypeConfiguration<Country>
   {
      public void Configure(EntityTypeBuilder<Country> builder)
      {
         builder.ToTable("Country");

         builder.HasKey(c => c.Id);

         builder.Property(c => c.Id)
            .IsRequired();

         builder.Property(c => c.UId)
            .IsRequired();

         builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

         builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);
      }
   }
}
