using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Categories;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Categories.Create
{
   public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, Category>
   {

      private readonly IMapper _mapper;

      public CreateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      public async Task<Category> Handle(CreateCommand request, CancellationToken cancellationToken)
      {
         var categoryWithSameName =
            await Ctx.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == request.CreateCategory.Name.ToLower(),
               cancellationToken);

         if (categoryWithSameName != null)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, "A category with the same name already exists.");

         var category = Domain.Entities.Category.Create(request.CreateCategory.Name, request.CreateCategory.Description);

         await Ctx.Categories.AddAsync(category, cancellationToken);
         await Ctx.SaveChangesAsync(cancellationToken);

         return _mapper.Map<Category>(category);
      }
   }
}
