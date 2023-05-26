using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class BeerFlavourConfiguration : IEntityTypeConfiguration<BeerFlavour>
   {
      public void Configure(EntityTypeBuilder<BeerFlavour> builder)
      {
         builder.ToTable("BeerFlavour");

         builder.HasKey(bf => new { bf.BeerId, bf.FlavourId });

         builder.HasOne(bf => bf.Beer)
            .WithMany(b => b.BeerFlavours)
            .HasForeignKey(bf => bf.BeerId);

         builder.HasOne(bf => bf.Flavour);
      }
   }
}
