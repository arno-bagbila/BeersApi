using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Input.Flavours.Update;
using BeersApi.Models.Output.Flavours;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Flavours.Update
{
   public class UpdateCommandHandler : BaseHandler, IRequestHandler<UpdateCommand, Flavour>
   {
      private readonly IMapper _mapper;

      public UpdateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      public async Task<Flavour> Handle(UpdateCommand command, CancellationToken cancellationToken)
      {
         var flavour = await Ctx.Flavours.FirstOrDefaultAsync(f => f.Id == command.FlavourId, cancellationToken)
            .ConfigureAwait(false);

         if (flavour == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Could not find flavour with id {command.FlavourId}!");

         if (flavour.Name != command.UpdateFlavour.Name)
         {
            var existingFlavour = await Ctx.Flavours
               .FirstOrDefaultAsync(f => f.Name.ToLower() == command.UpdateFlavour.Name.ToLower(), cancellationToken)
               .ConfigureAwait(false);

            if (existingFlavour != null)
               throw BeersApiException.Create(BeersApiException.InvalidDataCode, $"There is already a flavour with the name {command.UpdateFlavour.Name}.");
         }

         flavour = UpdateFlavour(flavour, command.UpdateFlavour);

         Ctx.Flavours.Update(flavour);
         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<Flavour>(flavour);
      }

      /// <summary>
      /// Set new values
      /// </summary>
      /// <param name="flavour">A flavour</param>
      /// <param name="updatedValues">new values for the flavour</param>
      /// <returns>the updated <paramref name="flavour"/></returns>
      /// <exception cref="ArgumentNullException"> throws if at least one of the value <paramref name="updatedValues"/> is not valid</exception>
      private Domain.Entities.Flavour UpdateFlavour(Domain.Entities.Flavour flavour, UpdateFlavour updatedValues)
      {
         flavour.Name = updatedValues.Name;
         flavour.Description = updatedValues.Description;

         return flavour;
      }
   }
}
