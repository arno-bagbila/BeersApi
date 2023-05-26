using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Entities
{
   public class Beer
   {
      public Beer()
      {
         UId = Guid.NewGuid();
         Images = new List<Image>();
         BeerFlavours = new List<BeerFlavour>();
         DateRegistered = DateTime.Now;
         Comments = new List<Comment>();
      }

      #region Data

      private string _name;
      private string _description;
      private double _alcoholLevel;
      private double _tiwooRating;
      private string _logoUrl;
      private Category _category;
      private Color _color;
      private Country _country;

      #endregion


      public int Id { get; private set; }

      /// <summary>
      /// beer unique identifier
      /// </summary>
      public Guid UId { get; private set; }

      /// <summary>
      /// description of the flavour
      /// </summary>
      public string Description
      {
         get => _description;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Description));
            _description = value;
         }
      }

      /// <summary>
      /// Beer name
      /// </summary>
      public string Name
      {
         get => _name;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(Name));
            _name = value;
         }
      }

      /// <summary>
      /// Beer alcohol level, between 0 and 100
      /// </summary>
      public double AlcoholLevel
      {
         get => _alcoholLevel;
         set
         {
            value.ThrowIfZero(nameof(AlcoholLevel));
            _alcoholLevel = value;
         }
      }

      /// <summary>
      /// Beer rating by Tiwoo
      /// </summary>
      public double TiwooRating
      {
         get => _tiwooRating;
         set
         {
            value.ThrowIfZero(nameof(TiwooRating));
            _tiwooRating = value;
         }
      }

      /// <summary>
      /// Beer category
      /// </summary>
      public Category Category
      {
         get => _category;
         set
         {
            value.ThrowIfNull(nameof(Category));
            _category = value;
         }
      }

      /// <summary>
      /// Beer color
      /// </summary>
      public Color Color
      {
         get => _color;
         set
         {
            value.ThrowIfNull(nameof(Color));
            _color = value;
         }
      }

      /// <summary>
      /// Beer logo url
      /// </summary>
      public string LogoUrl
      {
         get => _logoUrl;
         set
         {
            value.ThrowIfNullOrWhiteSpace(nameof(LogoUrl));
            _logoUrl = value;
         }
      }

      public Country Country
      {
         get => _country;
         set
         {
            value.ThrowIfNull(nameof(Country));
            _country = value;
         }
      }

      public DateTime DateRegistered { get; private set; }

      /// <summary>
      /// Can only be updated via <see cref="SetFlavours"/> and <see cref="RemoveFlavours"/>
      /// </summary>
      public ICollection<BeerFlavour> BeerFlavours { get; private set; }

      /// <summary>
      /// Beer images, can only be set and update via <see cref="SetImages"/>
      /// </summary>
      public ICollection<Image> Images { get; }

      /// <summary>
      /// Beers comments
      /// </summary>
      public ICollection<Comment> Comments { get; }

      /// <summary>
      /// Check if Beer can be created
      /// </summary>
      /// <param name="name">beer name</param>
      /// <param name="description">beer description</param>
      /// <param name="logoUrl">beer logo url</param>
      /// <param name="alcoholLevel">beer alcohol level</param>
      /// <param name="tiwooRating">beer rating</param>
      /// <param name="category">beer category</param>
      /// <param name="color">beer color</param>
      /// <param name="country">beer country</param>
      /// <returns> <see cref="ValidationResult"/></returns>
      public static ValidationResult CanCreate(string name, string description, string logoUrl, double alcoholLevel,
         double tiwooRating, Category category, Color color, Country country)
      {
         var errors = new List<(string Name, string Msg)>();

         name.CheckMandatory(nameof(Name), errors);
         description.CheckMandatory(nameof(Description), errors);
         logoUrl.CheckMandatory(nameof(LogoUrl), errors);
         alcoholLevel.CheckMandatory(nameof(AlcoholLevel), errors);
         tiwooRating.CheckMandatory(nameof(TiwooRating), errors);
         category.CheckMandatory(nameof(Category), errors);
         color.CheckMandatory(nameof(Color), errors);
         country.CheckMandatory(nameof(Country), errors);

         return errors.ToValidationResult();
      }

      /// <summary>
      /// Create a Beer
      /// </summary>
      /// <param name="name">beer name</param>
      /// <param name="description">beer description</param>
      /// <param name="logoUrl"> beer logo url</param>
      /// <param name="alcoholLevel">beer alcohol level</param>
      /// <param name="tiwooRating">beer rating by tiwoo</param>
      /// <param name="category">beer category</param>
      /// <param name="color">beer color</param>
      /// <param name="country">beer country</param>
      /// <param name="flavours">beer flavours</param>
      /// <returns>a <see cref="Beer"/></returns>
      /// <exception cref="BeersApiException"></exception>
      public static Beer Create(string name, string description, string logoUrl, double alcoholLevel,
         double tiwooRating, Category category, Color color, Country country)
      {
         var validationResult =
            CanCreate(name, description, logoUrl, alcoholLevel, tiwooRating, category, color, country);
         if (validationResult != ValidationResult.Success)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, validationResult.ErrorMessage,
               validationResult.MemberNames);

         return new Beer
         {
            Name = name,
            Description = description,
            LogoUrl = logoUrl,
            AlcoholLevel = alcoholLevel,
            TiwooRating = tiwooRating,
            Country = country,
            Color = color,
            Category = category
         };
      }

      /// <summary>
      /// Add to <see cref="BeerFlavours"/>
      /// </summary>
      /// <param name="flavours">flavours to add to the beer</param>
      public void SetFlavours(IEnumerable<Flavour> flavours)
      {
         //Remove the flavours we had for this beer before setting new values
         BeerFlavours.Clear();

         //only add the flavours that are not already in the beer
         foreach (var flavour in flavours)
         {
            var existingBeerFlavour =
               BeerFlavours.FirstOrDefault(bf => bf.FlavourId == flavour.Id && bf.BeerId == Id);

            if (existingBeerFlavour == null)
               BeerFlavours.Add(BeerFlavour.Create(this, flavour));
         }
      }

      /// <summary>
      /// Remove beerflavours to <see cref="BeerFlavours"/>
      /// </summary>
      /// <param name="flavours">flavours to remove</param>
      public void RemoveFlavours(IEnumerable<Flavour> flavours)
      {
         //only remove the flavours that are part of BeerFlavours
         foreach (var flavour in flavours)
         {
            var existingBeerFlavour =
               BeerFlavours.FirstOrDefault(bf => bf.FlavourId == flavour.Id && bf.BeerId == Id);

            if (existingBeerFlavour != null)
               BeerFlavours.Remove(existingBeerFlavour);
         }
      }

      /// <summary>
      /// Manipulate <see cref="Images"/>
      /// </summary>
      /// <param name="images">images to add or update</param>
      public void SetImages(IEnumerable<Image> images)
      {
         var imagesUrls = Images.Select(i => i.ImageUrl);

         foreach (var image in images)
         {
            if (!imagesUrls.Contains(image.ImageUrl))
               Images.Add(image);
         }
      }

      public void AddComment(Comment comment)
      {
         if (comment.Beer.Id != Id)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, $"{nameof(Comments)} - Comment beer id is different from the beer id");
         Comments.Add(comment);
      }

      /// <summary>
      /// Delete beer flavour, before deleting beer
      /// </summary>
      public void DeleteBeerFlavours()
      {
         BeerFlavours.Clear();
      }
   }
}
