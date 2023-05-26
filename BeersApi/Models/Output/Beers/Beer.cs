using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Category = BeersApi.Models.Output.Categories.Category;
using Color = BeersApi.Models.Output.Colors.Color;
using Country = BeersApi.Models.Output.Countries.Country;
using Flavour = BeersApi.Models.Output.Flavours.Flavour;
using Image = BeersApi.Models.Output.Images.Image;

namespace BeersApi.Models.Output.Beers
{
   /// <summary>
   /// Output model for <see cref="Domain.Entities.Beer"/>
   /// </summary>
   public class Beer
   {

      /// <summary>
      /// Beer Id
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Beer Unique Id
      /// </summary>
      public Guid UId { get; set; }

      /// <summary>
      /// Beer Name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Beer Description
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// The alcohol level in the Beer
      /// </summary>
      public double AlcoholLevel { get; set; }

      /// <summary>
      /// Rating of Beer by Tiwoo
      /// </summary>
      public double TiwooRating { get; set; }

      /// <summary>
      /// Category to which the beer belongs
      /// </summary>
      public Category Category { get; set; }

      /// <summary>
      /// Color of the beer
      /// </summary>
      public Color Color { get; set; }

      /// <summary>
      /// Country of origin of the beer
      /// </summary>
      public Country Country { get; set; }

      /// <summary>
      /// Flavours of the beer
      /// </summary>
      public IEnumerable<Flavour> Flavours { get; set; }

      /// <summary>
      /// Images of the beer
      /// </summary>
      public IEnumerable<Image> Images { get; set; }

      /// <summary>
      /// Comments of the beer
      /// </summary>
      public IEnumerable<Comment> Comments { get; set; }

      /// <summary>
      /// Beer logo
      /// </summary>
      public string LogoUrl { get; set; }


      /// <summary>
      /// Beer registration date
      /// </summary>
      public DateTime DateRegistered { get; set; }
   }

   public class BeerMappingProfile : Profile
   {
      public BeerMappingProfile()
      {
         CreateMap<Domain.Entities.Beer, Beer>()
            .ForMember(b => b.Flavours, cfg =>
               cfg.MapFrom(b => b.BeerFlavours.Select(bf => bf.Flavour)));

      }
   }
}
