using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Categories;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Categories.Search
{
   public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<Category>>
   {
      private readonly IMapper _mapper;

      public SearchQueryHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx) => _mapper = mapper;

      public async Task<IEnumerable<Category>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
      {
         var categoriesQuery = Ctx.Categories.AsQueryable();

         if (searchQuery.Id.HasValue)
            categoriesQuery = categoriesQuery.Where(c => c.Id == searchQuery.Id.Value);


         var categories = await categoriesQuery
            .OrderBy(x => x.Name)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         return _mapper.Map<IEnumerable<Category>>(categories);
      }
   }
}
