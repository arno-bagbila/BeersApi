using BeersApi.Models.Output.Categories;
using MediatR;
using System.Collections.Generic;

namespace BeersApi.Features.Categories.Search
{
   public class SearchQuery : IRequest<IEnumerable<Category>>
   {
      /// <summary>
      /// Category Id
      /// </summary>
      public int? Id { get; set; }
   }
}
