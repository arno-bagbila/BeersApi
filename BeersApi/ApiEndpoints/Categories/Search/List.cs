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
   public class List : BaseAsyncEndpoint
      .WithRequest<SearchQuery>
      .WithResponse<IEnumerable<Category>>
   {

      private readonly IMapper _mapper;
      private readonly IBeersApiContext _ctx;

      public List(IMapper mapper, IBeersApiContext ctx)
      {
         _mapper = mapper;
         _ctx = ctx;
      }

      [HttpGet("/categories2")]
      [SwaggerOperation(
         Summary = "Get list of categories",
         Description = "Get list of categories",
         OperationId = "Category.List",
         Tags = new[] { "Categories2" })]
      public override async Task<ActionResult<IEnumerable<Category>>> HandleAsync(SearchQuery searchQuery, CancellationToken cancellationToken = new CancellationToken())
      {
         var categoriesQuery = _ctx.Categories.AsQueryable();

         if (searchQuery.Id.HasValue)
            categoriesQuery = categoriesQuery.Where(c => c.Id == searchQuery.Id.Value);


         var categories = await categoriesQuery
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         var result = _mapper.Map<IEnumerable<Category>>(categories);

         return Ok(result);
      }
   }
}
