using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Categories;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Categories.Delete
{
   public class DeleteCommandHandler : BaseHandler, IRequestHandler<DeleteCommand, Category>
   {
      #region Data

      private readonly IMapper _mapper;

      #endregion

      #region Constructors

      public DeleteCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      #endregion

      #region IRequestHandler

      public async Task<Category> Handle(DeleteCommand request, CancellationToken cancellationToken)
      {
         var category = await Ctx.Categories.FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

         if (category == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Category with id {request.CategoryId} could not be found.");

         Ctx.Categories.Remove(category);
         await Ctx.SaveChangesAsync(cancellationToken);

         return _mapper.Map<Category>(category);
      }

      #endregion

   }
}
