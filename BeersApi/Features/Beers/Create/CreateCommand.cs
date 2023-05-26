using BeersApi.Models.Output.Beers;
using MediatR;

namespace BeersApi.Features.Beers.Create
{
   public class CreateCommand : IRequest<Beer>
   {
      public Models.Input.Beers.Create.CreateBeer Beer { get; set; }
   }
}
