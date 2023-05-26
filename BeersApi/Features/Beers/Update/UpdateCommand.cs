using BeersApi.Models.Input.Beers.Update;
using MediatR;

namespace BeersApi.Features.Beers.Update
{
   public class UpdateCommand : IRequest<Models.Output.Beers.Beer>
   {
      public int BeerId { get; set; }

      public UpdateBeer Beer { get; set; }
   }
}
