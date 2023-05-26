using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class ColorConfiguration : IEntityTypeConfiguration<Color>
   {
      public void Configure(EntityTypeBuilder<Color> builder)
      {
         builder.ToTable("Color");

         builder.HasKey(c => c.Id);

         builder.Property(c => c.Id);

         builder.Property(c => c.UId)
            .IsRequired();

         builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
      }
   }
}
