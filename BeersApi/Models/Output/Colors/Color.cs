using AutoMapper;
using System;

namespace BeersApi.Models.Output.Colors
{
   /// <summary>
   /// Output model for <see cref="Domain.Entities.Color"/>
   /// </summary>
   public class Color : IEntity
   {
      /// <summary>
      /// Color Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Color name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Unique identifier for the color
      /// </summary>
      public Guid UId { get; set; }
   }

   public class ColorMappingProfile : Profile
   {
      public ColorMappingProfile()
      {
         CreateMap<Domain.Entities.Color, Color>().ReverseMap();
      }
   }
}
