using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Countries;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Countries.Search
{
   public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<Country>>
   {
      #region Data

      private readonly IMapper _mapper;

      #endregion

      #region Constructors

      public SearchQueryHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx)
      {
         _mapper = mapper;
      }

      #endregion

      #region IRequestHandler

      /// <summary>
      /// Get specific <see cref="Domain.Entities.Country"/>
      /// </summary>
      /// <param name="query">Request we get from the controller</param>
      /// <param name="cancellationToken"> A <see cref="CancellationToken"/> for the query</param>
      /// <returns>List of <see cref="Country"/></returns>
      public async Task<IEnumerable<Country>> Handle(SearchQuery query, CancellationToken cancellationToken)
      {
         var countriesQuery = Ctx.Countries.AsQueryable();

         if (query.CountryId.HasValue)
            countriesQuery = countriesQuery.Where(c => c.Id == query.CountryId);

         var countries = await countriesQuery
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         return _mapper.Map<IEnumerable<Country>>(countries);
      }

      #endregion

   }
}
