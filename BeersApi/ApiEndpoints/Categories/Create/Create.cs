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
    public class Create(IMapper mapper, IBeersApiContext ctx) : EndpointBaseAsync
        .WithRequest<CreateCommand>
        .WithActionResult<Category>
    {
        [HttpPost(CreateCommand.ROUTE)]
        [SwaggerOperation(
           Summary = "Creates a new category",
           Description = "Creates a new category",
           OperationId = "Category.Create",
           Tags = ["Categories2"])]
        public override async Task<ActionResult<Category>> HandleAsync([FromBody] CreateCommand request, CancellationToken cancellationToken)
        {
            var categoryWithSameName =
               await ctx.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == request.CreateCategory.Name.ToLower(),
                  cancellationToken);

            if (categoryWithSameName != null)
                throw BeersApiException.Create(BeersApiException.InvalidDataCode, "A category with the same name already exists.");

            var category = Domain.Entities.Category.Create(request.CreateCategory.Name, request.CreateCategory.Description);

            await ctx.Categories.AddAsync(category, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);

            var result = mapper.Map<Category>(category);

            return Ok(result);
        }
    }
}
