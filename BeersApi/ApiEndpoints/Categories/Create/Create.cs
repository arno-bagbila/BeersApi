using Ardalis.ApiEndpoints;
using AutoMapper;
using BeersApi.Models.Output.Categories;
using DataAccess;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.ApiEndpoints.Categories.Create
{
   public class Create : BaseAsyncEndpoint
      .WithRequest<CreateCommand>
      .WithResponse<Category>
   {

      private readonly IMapper _mapper;
      private readonly IBeersApiContext _ctx;

      public Create(IMapper mapper, IBeersApiContext ctx)
      {
         _mapper = mapper;
         _ctx = ctx;
      }

      [HttpPost(CreateCommand.ROUTE)]
      [SwaggerOperation(
         Summary = "Creates a new category",
         Description = "Creates a new category",
         OperationId = "Category.Create",
         Tags = new[] { "Categories2" })]
      public override async Task<ActionResult<Category>> HandleAsync([FromBody] CreateCommand request, CancellationToken cancellationToken)
      {
         var categoryWithSameName =
            await _ctx.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == request.CreateCategory.Name.ToLower(),
               cancellationToken);

         if (categoryWithSameName != null)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode, "A category with the same name already exists.");

         var category = Domain.Entities.Category.Create(request.CreateCategory.Name, request.CreateCategory.Description);

         await _ctx.Categories.AddAsync(category, cancellationToken);
         await _ctx.SaveChangesAsync(cancellationToken);

         var result = _mapper.Map<Category>(category);

         return Ok(result);
      }
   }
}
