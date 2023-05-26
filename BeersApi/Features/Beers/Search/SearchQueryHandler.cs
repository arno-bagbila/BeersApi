using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Beers;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Beers.Search
{
   public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<Beer>>
   {
      private readonly IMapper _mapper;

      public SearchQueryHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      public async Task<IEnumerable<Beer>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
      {
         var beerFilter = searchQuery.BeersFilter;

         var beersQuery = Ctx.Beers
            .Include(b => b.Category)
            .Include(b => b.Color)
            .Include(b => b.Country)
            .Include(b => b.BeerFlavours).ThenInclude(bf => bf.Flavour)
            .AsQueryable();

         if (searchQuery.Id.HasValue)
         {
            beersQuery = beersQuery.Where(b => b.Id == searchQuery.Id.Value);
         }
         else
         {
            if (beerFilter?.CategoryIds?.Any() ?? false)
               beersQuery = beersQuery.Where(b => beerFilter.CategoryIds.Contains(b.Category.Id));

            if (beerFilter?.ColorIds?.Any() ?? false)
               beersQuery = beersQuery.Where(b => beerFilter.ColorIds.Contains(b.Color.Id));

            if (beerFilter?.CountryIds?.Any() ?? false)
               beersQuery = beersQuery.Where(b => beerFilter.CountryIds.Contains(b.Country.Id));
         }

         var beers = await beersQuery
            .OrderBy(b => b.Name)
            .ThenBy(b => b.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         return _mapper.Map<IEnumerable<Beer>>(beers);
      }
   }
}
