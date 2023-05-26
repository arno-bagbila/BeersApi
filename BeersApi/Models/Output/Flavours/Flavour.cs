using AutoMapper;
using System;

namespace BeersApi.Models.Output.Flavours
{
   public class Flavour : IEntity
   {

      public int Id { get; set; }

      public Guid UId { get; set; }

      public string Name { get; set; }

      public string Description { get; set; }
   }

   public class FlavourMappingProfile : Profile
   {
      public FlavourMappingProfile()
      {
         CreateMap<Domain.Entities.Flavour, Flavour>().ReverseMap();
      }
   }
}
