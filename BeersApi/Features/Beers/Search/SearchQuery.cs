using BeersApi.Models.Input.Beers.Create;
using BeersApi.Models.Output.Beers;
using MediatR;
using System.Collections.Generic;

namespace BeersApi.Features.Beers.Search
{
   public class SearchQuery : IRequest<IEnumerable<Beer>>
   {
      public int? Id { get; set; }

      public BeersFilter BeersFilter { get; set; }
   }
}
