using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models;
using BeersApi.Models.Input.Beers.Update;
using DataAccess;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Beer = BeersApi.Models.Output.Beers.Beer;

namespace BeersApi.Features.Beers.Update
{
   public class UpdateCommandHandler : BaseHandler, IRequestHandler<UpdateCommand, Beer>
   {
      #region Data

      private readonly IMapper _mapper;

      #endregion

      #region Constructors

      public UpdateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      #endregion

      #region IRequestHandler

      public async Task<Beer> Handle(UpdateCommand command, CancellationToken cancellationToken)
      {
         var beer = await Ctx.Beers
            .Include(b => b.BeerFlavours)
            .FirstOrDefaultAsync(b => b.Id == command.BeerId, cancellationToken)
            .ConfigureAwait(false);

         if (beer is null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Beer with id {command.BeerId} could not be found.");

         var beerUpdated = command.Beer;

         if (beerUpdated.Name != beer.Name)
         {
            var beerWithExistingName =
               await Ctx.Beers.FirstOrDefaultAsync(b => b.Name.ToLower() == beerUpdated.Name.ToLower(),
                  cancellationToken);

            if (beerWithExistingName != null)
               throw BeersApiException.Create(BeersApiException.InvalidDataCode,
                  $"A beer with the name '{beerUpdated.Name}' already exists.");
         }

         var category = await Ctx.Categories
            .FirstOrDefaultAsync(c => c.Id == beerUpdated.CategoryId, cancellationToken)
            .ConfigureAwait(false);

         if (category is null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Category with id {beerUpdated.CategoryId} could not be found.");

         var color = await Ctx.Colors
            .FirstOrDefaultAsync(c => c.Id == beerUpdated.ColorId, cancellationToken)
            .ConfigureAwait(false);

         if (color is null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Color with id {beerUpdated.ColorId} could not be found.");

         var country = await Ctx.Countries
            .FirstOrDefaultAsync(c => c.Id == beerUpdated.CountryId, cancellationToken)
            .ConfigureAwait(false);

         if (country == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Country with id {beerUpdated.CountryId} could not be found.");

         var flavours = await FlavoursValidation(beerUpdated.FlavourIds, cancellationToken).ConfigureAwait(false);

         beer = Update(beer, beerUpdated, country, category, color);
         Ctx.Beers.Update(beer);

         if (flavours.Any())
            beer.SetFlavours(flavours);

         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<Beer>(beer);
      }

      #endregion

      #region Private Methods

      private async Task<IEnumerable<Flavour>> FlavoursValidation(IEnumerable<int> flavourIds, CancellationToken cancellationToken)
      {
         var errors = new List<ErrorDetailsFeatures>();

         var flavours = await Ctx.Flavours
            .Where(f => flavourIds.Contains(f.Id))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         if (!flavours.Any())
            throw BeersApiException.Create(BeersApiException.NotFound, $"Could not find {nameof(flavours)} with the following {flavourIds}");

         foreach (var flavourId in flavourIds)
         {
            if (!flavours.Select(f => f.Id).Contains(flavourId))
               errors.Add(new ErrorDetailsFeatures { Message = $"Could not find flavour for id {flavourId}" });
         }

         if (errors.Any())
            throw BeersApiException.Create(BeersApiException.NotFound, string.Join(" | ", errors.Select(e => e.Message)));

         return flavours;
      }

      /// <summary>
      /// set new values
      /// </summary>
      /// <param name="beer">A beer</param>
      /// <param name="updatedValues">new values to set for the beer</param>
      /// <param name="country">updated country</param>
      /// <param name="category">updated category</param>
      /// <param name="color"> updated color</param>
      /// <returns>The updated <paramref name="beer"></paramref></returns>
      /// <exception cref="BeersApiException"> throws if at least one of the value <paramref name="updatedValues"/> is not valid</exception>
      private Domain.Entities.Beer Update(Domain.Entities.Beer beer, UpdateBeer updatedValues, Country country, Category category, Color color)
      {
         beer.Name = updatedValues.Name;
         beer.AlcoholLevel = updatedValues.AlcoholLevel;
         beer.Description = updatedValues.Description;
         beer.LogoUrl = updatedValues.LogoUrl;
         beer.TiwooRating = updatedValues.TiwooRating;
         beer.Country = country;
         beer.Category = category;
         beer.Color = color;

         return beer;
      }

      #endregion
   }
}
