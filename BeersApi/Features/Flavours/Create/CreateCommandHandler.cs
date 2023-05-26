using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Flavours;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Flavours.Create
{
   public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, Flavour>
   {
      private readonly IMapper _mapper;

      public CreateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      public async Task<Flavour> Handle(CreateCommand command, CancellationToken cancellationToken)
      {
         var existingFlavour = await Ctx.Flavours
            .FirstOrDefaultAsync(f => f.Name.ToLower() == command.CreateFlavour.Name.ToLower(), cancellationToken)
            .ConfigureAwait(false);

         if (existingFlavour != null)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, $"A flavour with the name {command.CreateFlavour.Name} already exists!");

         var flavour = Domain.Entities.Flavour.Create(command.CreateFlavour.Name, command.CreateFlavour.Description);

         await Ctx.Flavours.AddAsync(flavour, cancellationToken).ConfigureAwait(false);
         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<Flavour>(flavour);
      }
   }
}
