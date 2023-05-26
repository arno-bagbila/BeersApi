using AutoMapper;
using System;

namespace BeersApi.Models.Output.Countries
{
   public class Country : IEntity
   {
      /// <summary>
      /// Country Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Unique identifier for the country
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Color name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Code of the Country
      /// </summary>
      public string Code { get; set; }
   }

   public class CountryMappingProfile : Profile
   {
      public CountryMappingProfile()
      {
         CreateMap<Domain.Entities.Country, Country>().ReverseMap();
      }
   }
}
