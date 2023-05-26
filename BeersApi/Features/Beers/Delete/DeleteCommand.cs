using BeersApi.Models.Output.Beers;
using MediatR;

namespace BeersApi.Features.Beers.Delete
{
   public class DeleteCommand : IRequest<Beer>
   {
      public int BeerId { get; set; }
   }
}
