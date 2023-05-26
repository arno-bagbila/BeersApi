using BeersApi.Models.Output.Countries;
using MediatR;
using System.Collections.Generic;

namespace BeersApi.Features.Countries.Search
{
   public class SearchQuery : IRequest<IEnumerable<Country>>
   {
      public int? CountryId { get; set; }
   }
}
