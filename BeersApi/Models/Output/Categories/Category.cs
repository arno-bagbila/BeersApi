using AutoMapper;
using System;

namespace BeersApi.Models.Output.Categories
{
   /// <summary>
   /// Output model for <see cref="Domain.Entities.Category"/>
   /// </summary>
   public class Category
   {

      /// <summary>
      /// Category Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Category unique Id
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Category Name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Category Description
      /// </summary>
      public string Description { get; set; }
   }

   public class CategoryMappingProfile : Profile
   {
      public CategoryMappingProfile()
      {
         CreateMap<Domain.Entities.Category, Category>().ReverseMap();
      }
   }
}
