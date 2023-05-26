using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Colors;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Colors.Search
{
   public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<Color>>
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

      public async Task<IEnumerable<Color>> Handle(SearchQuery query, CancellationToken cancellationToken)
      {
         var colorsQuery = Ctx.Colors.AsQueryable();

         if (query.ColorId.HasValue)
         {
            colorsQuery = colorsQuery.Where(c => c.Id == query.ColorId.Value);
         }

         var colors = await colorsQuery
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);



         return _mapper.Map<IEnumerable<Color>>(colors);
      }

      #endregion


   }
}
