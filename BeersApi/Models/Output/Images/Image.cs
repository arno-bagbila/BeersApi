using AutoMapper;
using System;

namespace BeersApi.Models.Output.Images
{
   public class Image
   {
      /// <summary>
      /// Image Id
      /// </summary>
      public Guid Id { get; set; }

      /// <summary>
      /// Beer which image we have Id
      /// </summary>
      public Guid BeerId { get; set; }

      /// <summary>
      /// Image title
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Image Filename
      /// </summary>
      public string ImageUrl { get; set; }

      public DateTime DateRegistered { get; set; }
   }

   public class ImageMappingProfile : Profile
   {
      public ImageMappingProfile()
      {
         CreateMap<Domain.Entities.Image, Image>().ReverseMap();
      }
   }
}
