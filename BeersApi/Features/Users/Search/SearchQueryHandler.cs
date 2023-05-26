using AutoMapper;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Users;
using DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Users.Search
{
   public class SearchQueryHandler : BaseHandler, IRequestHandler<SearchQuery, IEnumerable<User>>
   {
      private readonly IMapper _mapper;
      public SearchQueryHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx) => _mapper = mapper;

      public async Task<IEnumerable<User>> Handle(SearchQuery searchQuery, CancellationToken cancellationToken)
      {
         var usersQuery = Ctx.Users.AsQueryable();

         if (!string.IsNullOrWhiteSpace(searchQuery.Email))
            usersQuery = usersQuery.Where(u => u.Email == searchQuery.Email);

         var users = await usersQuery
            .OrderBy(u => u.Id)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

         return _mapper.Map<IEnumerable<User>>(users);
      }
   }
}
