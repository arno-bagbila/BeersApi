using BeersApi.Models.Output.Flavours;
using MediatR;
using System.Collections.Generic;

namespace BeersApi.Features.Flavours.Search
{
   public class SearchQuery : IRequest<IEnumerable<Flavour>>
   {
      public int? FlavourId { get; set; }
   }
}
