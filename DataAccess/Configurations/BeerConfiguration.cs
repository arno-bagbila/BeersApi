using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class BeerConfiguration : IEntityTypeConfiguration<Beer>
   {
      public void Configure(EntityTypeBuilder<Beer> builder)
      {
         builder.ToTable("Beer");

         builder.HasKey(b => b.Id);

         builder.Property(b => b.UId)
            .IsRequired();

         builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50);

         builder.Property(b => b.Description)
            .IsRequired()
            .HasMaxLength(3000);

         builder.Property(b => b.LogoUrl)
            .IsRequired()
            .HasMaxLength(2048);
         //.HasConversion(v => v.ToString(), v => new Uri(v))

         builder.Property(b => b.TiwooRating)
               .IsRequired();

         builder.Property(b => b.AlcoholLevel)
            .IsRequired();

         builder.HasOne(b => b.Category)
            .WithMany()
            .IsRequired();

         builder.HasOne(b => b.Color)
            .WithMany()
            .IsRequired();
         builder.HasOne(b => b.Country)
            .WithMany()
            .IsRequired();

         builder.HasMany(b => b.Images)
            .WithOne(i => i.Beer)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

         builder.HasMany(b => b.Comments)
            .WithOne(c => c.Beer)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
      }
   }
}
