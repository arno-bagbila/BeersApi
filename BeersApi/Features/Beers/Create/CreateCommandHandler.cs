using AutoMapper;
using BeersApi.Infrastructure;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Beer = BeersApi.Models.Output.Beers.Beer;

namespace BeersApi.Features.Beers.Create
{
   public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, Beer>
   {

      #region Data

      private readonly IMapper _mapper;

      #endregion

      #region Constructors

      public CreateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      #endregion

      #region IRequestHandler

      public async Task<Beer> Handle(CreateCommand command, CancellationToken cancellationToken)
      {

         var existingBeer = await Ctx
            .Beers
            .FirstOrDefaultAsync(b => b.Name.ToLower() == command.Beer.Name.ToLower(), cancellationToken: cancellationToken)
            .ConfigureAwait(false);

         if (existingBeer != null)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode,
               $"A beer with the name {command.Beer.Name} already exists.");

         var category = await Ctx
            .Categories
            .FirstOrDefaultAsync(c => c.Id == command.Beer.CategoryId, cancellationToken)
            .ConfigureAwait(false);

         if (category is null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Could not find category with Id {command.Beer.CategoryId}.");

         var color = await Ctx.Colors
            .FirstOrDefaultAsync(c => c.Id == command.Beer.ColorId, cancellationToken)
            .ConfigureAwait(false);

         if (color is null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Could not find color with Id {command.Beer.ColorId}.");

         var country = await Ctx.Countries.FirstOrDefaultAsync(c => c.Id == command.Beer.CountryId, cancellationToken)
            .ConfigureAwait(false);

         if (country == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Could not find country with Id {command.Beer.CountryId}.");

         var flavours = await Ctx.Flavours
            .Where(f => command.Beer.FlavourIds.Contains(f.Id))
            .ToListAsync(cancellationToken);

         if (!flavours.Any())
            throw BeersApiException.Create(BeersApiException.NotFound, $"Could not find flavours with Ids {command.Beer.FlavourIds}.");

         var beer = Domain.Entities.Beer.Create(command.Beer.Name, command.Beer.Description, command.Beer.LogoUrl, command.Beer.AlcoholLevel,
            command.Beer.TiwooRating, category, color, country);
         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         beer.SetFlavours(flavours);

         await Ctx.Beers.AddAsync(beer, cancellationToken).ConfigureAwait(false);
         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<Beer>(beer);
      }

      #endregion


   }
}
