using BeersApi.Models.Output.Users;
using MediatR;
using System.Collections.Generic;

namespace BeersApi.Features.Users.Search
{
   public class SearchQuery : IRequest<IEnumerable<User>>
   {
      /// <summary>
      /// User email
      /// </summary>
      public string Email { get; set; }
   }
}
