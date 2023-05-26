using Domain.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class UserConfiguration : IEntityTypeConfiguration<User>
   {
      public void Configure(EntityTypeBuilder<User> builder)
      {
         builder.ToTable("User");

         builder.HasKey(u => u.Id);

         builder.Property(u => u.Id);

         builder.Property(u => u.UId)
            .IsRequired();

         builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

         builder.Property(u => u.Role)
            .IsRequired();

         builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(256);
      }
   }
}
