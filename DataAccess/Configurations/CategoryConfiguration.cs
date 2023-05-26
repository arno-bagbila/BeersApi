﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
   public class CategoryConfiguration : IEntityTypeConfiguration<Category>
   {
      public void Configure(EntityTypeBuilder<Category> builder)
      {
         builder.ToTable("Category");

         builder.HasKey(c => c.Id);

         builder.Property(c => c.Id);

         builder.Property(c => c.UId)
            .IsRequired();

         builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

         builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(3000);
      }
   }
}
