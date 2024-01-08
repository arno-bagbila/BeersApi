using Ardalis.ApiEndpoints;
using AutoMapper;
using BeersApi.Models.Output.Categories;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.ApiEndpoints.Categories.Search
{
    public class List(IMapper mapper, IBeersApiContext ctx) : EndpointBaseAsync
        .WithRequest<SearchQuery>
        .WithActionResult<IEnumerable<Category>>
    {
        [HttpGet("/categories2")]
        [SwaggerOperation(
           Summary = "Get list of categories",
           Description = "Get list of categories",
           OperationId = "Category.List",
           Tags = ["Categories2"])]
        public override async Task<ActionResult<IEnumerable<Category>>> HandleAsync(SearchQuery searchQuery, CancellationToken cancellationToken = new CancellationToken())
        {
            var categoriesQuery = ctx.Categories.AsQueryable();

            if (searchQuery.Id.HasValue)
                categoriesQuery = categoriesQuery.Where(c => c.Id == searchQuery.Id.Value);


            var categories = await categoriesQuery
               .OrderBy(x => x.Name)
               .ThenBy(x => x.Id)
               .ToListAsync(cancellationToken)
               .ConfigureAwait(false);

            var result = mapper.Map<IEnumerable<Category>>(categories);

            return Ok(result);
        }
    }
}
