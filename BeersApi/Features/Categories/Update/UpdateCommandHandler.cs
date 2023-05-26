using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Input.Categories.Update;
using BeersApi.Models.Output.Categories;
using DataAccess;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Categories.Update
{
   public class UpdateCommandHandler : BaseHandler, IRequestHandler<UpdateCommand, Category>
   {
      private readonly IMapper _mapper;

      public UpdateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      public async Task<Category> Handle(UpdateCommand command, CancellationToken cancellationToken)
      {
         var category = await Ctx.Categories.FirstOrDefaultAsync(c => c.Id == command.CategoryId, cancellationToken)
            .ConfigureAwait(false);

         if (category == null)
            throw BeersApiException.Create(BeersApiException.NotFound, $"Category with id {command.CategoryId} could not be found.");

         if (command.UpdateCategory.Name != category.Name)
         {
            var categoryWithExistingName =
               await Ctx.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == command.UpdateCategory.Name.ToLower(),
                  cancellationToken);

            if (categoryWithExistingName != null)
               throw BeersApiException.Create(BeersApiException.InvalidDataCode,
                  $"A category with the name '{command.UpdateCategory.Name}' already exists.");
         }

         category = UpdateCategory(category, command.UpdateCategory);

         Ctx.Categories.Update(category);
         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<Category>(category);
      }

      /// <summary>
      /// Set new values
      /// </summary>
      /// <param name="category">A category</param>
      /// <param name="updatedValues">new values for the category</param>
      /// <returns>the updated <paramref name="category"/></returns>
      /// <exception cref="BeersApiException"> throws if at least one of the value <paramref name="updatedValues"/> is not valid</exception>
      private Domain.Entities.Category UpdateCategory(Domain.Entities.Category category, UpdateCategory updatedValues)
      {
         category.Name = updatedValues.Name;
         category.Description = updatedValues.Description;

         return category;
      }
   }
}
