using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Beers;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Beers.Delete
{
   public class DeleteCommandHandler : BaseHandler, IRequestHandler<DeleteCommand, Beer>
   {
      #region Data

      private readonly IMapper _mapper;

      #endregion

      public DeleteCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      public async Task<Beer> Handle(DeleteCommand command, CancellationToken cancellationToken)
      {
         var beer = await Ctx.Beers
            .Include(b => b.BeerFlavours).ThenInclude(bf => bf.Flavour)
            .Include(b => b.Category)
            .Include(b => b.Color)
            .Include(b => b.Country)
            .FirstOrDefaultAsync(b => b.Id == command.BeerId, cancellationToken)
            .ConfigureAwait(false);

         if (beer == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Beer with id {command.BeerId} could not be found.");

         Ctx.Beers.Remove(beer);
         await Ctx.SaveChangesAsync(cancellationToken);

         return _mapper.Map<Beer>(beer);
      }
   }
}
