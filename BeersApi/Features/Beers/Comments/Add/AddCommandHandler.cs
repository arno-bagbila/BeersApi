using AutoMapper;
using BeersApi.Infrastructure;
using DataAccess;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Beer = BeersApi.Models.Output.Beers.Beer;

namespace BeersApi.Features.Beers.Comments.Add
{
   public class AddCommandHandler : BaseHandler, IRequestHandler<AddCommand, Beer>
   {
      #region Data

      private readonly IMapper _mapper;

      #endregion

      #region Constructors

      public AddCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      #endregion


      public async Task<Beer> Handle(AddCommand command, CancellationToken cancellationToken)
      {
         var beer = await Ctx
            .Beers
            .Include(b => b.Category)
            .Include(b => b.Color)
            .Include(b => b.Country)
            .Include(b => b.BeerFlavours).ThenInclude(bf => bf.Flavour)
            .Include(b => b.Images)
            .FirstOrDefaultAsync(b => b.Id == command.Comment.BeerId, cancellationToken)
            .ConfigureAwait(false);

         if (beer is null)
            throw BeersApiException.Create(BeersApiException.NotFound,
               $@"Could not find beer with Id ""{command.Comment.BeerId}"" ");

         var user = await Ctx
            .Users
            .FirstOrDefaultAsync(u => u.UId == command.Comment.UserUId, cancellationToken);

         if (user is null)
            throw BeersApiException.Create(BeersApiException.NotFound,
               $@"Could not find beer with Id ""{command.Comment.BeerId}"" ");

         var comment = Comment.Create(command.Comment.Body, command.Comment.UserFirstName, beer,
            command.Comment.UserUId);

         beer.AddComment(comment);

         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<Beer>(beer);
      }
   }
}
