using BeersApi.Models.Output.Colors;
using MediatR;
using System.Collections.Generic;

namespace BeersApi.Features.Colors.Search
{
   public class SearchQuery : IRequest<IEnumerable<Color>>
   {
      public int? ColorId { get; set; }
   }
}
