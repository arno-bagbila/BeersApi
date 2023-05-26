using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Flavours;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Flavours.Delete
{
   public class DeleteCommandHandler : BaseHandler, IRequestHandler<DeleteCommand, Flavour>
   {
      #region Data

      private readonly IMapper _mapper;

      #endregion

      #region Constructors

      public DeleteCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx) => _mapper = mapper;

      #endregion

      public async Task<Flavour> Handle(DeleteCommand request, CancellationToken cancellationToken)
      {
         var flavour = await Ctx.Flavours.FirstOrDefaultAsync(c => c.Id == request.FlavourId, cancellationToken)
            .ConfigureAwait(false);

         if (flavour == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Flavour with id {request.FlavourId} could not be found.");

         Ctx.Flavours.Remove(flavour);
         await Ctx.SaveChangesAsync(cancellationToken);

         return _mapper.Map<Flavour>(flavour);
      }
   }
}
