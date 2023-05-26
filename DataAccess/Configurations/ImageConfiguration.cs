using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class ImageConfiguration : IEntityTypeConfiguration<Image>
   {
      public void Configure(EntityTypeBuilder<Image> builder)
      {
         builder.ToTable("Image");

         builder.HasKey(i => i.Id);

         builder.Property(i => i.UId);

         builder.Property(i => i.ImageUrl)
            .HasMaxLength(200)
            .IsRequired();

         builder.Property(i => i.Title)
            .HasMaxLength(50)
            .IsRequired();

         builder.HasOne(i => i.Beer)
            .WithMany(b => b.Images)
            .IsRequired();
      }
   }
}
