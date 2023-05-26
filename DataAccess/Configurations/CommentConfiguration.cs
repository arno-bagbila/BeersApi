using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class CommentConfiguration : IEntityTypeConfiguration<Comment>
   {
      public void Configure(EntityTypeBuilder<Comment> builder)
      {
         builder.ToTable("Comment");

         builder.HasKey(c => c.Id);

         builder.Property(c => c.Id);

         builder.HasOne(c => c.Beer)
            .WithMany(b => b.Comments)
            .IsRequired();

         builder.Property(c => c.Body)
            .IsRequired()
            .HasMaxLength(3000);

         builder.Property(c => c.UserFirstName)
            .IsRequired()
            .HasMaxLength(256);

         builder.Property(c => c.UserUId)
            .IsRequired();
      }
   }
}
