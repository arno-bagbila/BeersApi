using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Flavours;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Flavours.Search
{
   public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<Flavour>>
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

      public async Task<IEnumerable<Flavour>> Handle(SearchQuery query, CancellationToken cancellationToken)
      {
         var flavoursQuery = Ctx.Flavours.AsQueryable();

         if (query.FlavourId.HasValue)
            flavoursQuery = flavoursQuery.Where(f => f.Id == query.FlavourId);

         var flavours = await flavoursQuery
            .OrderBy(f => f.Name)
            .ThenBy(f => f.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         return _mapper.Map<IEnumerable<Flavour>>(flavours);
      }

      #endregion

   }
}
