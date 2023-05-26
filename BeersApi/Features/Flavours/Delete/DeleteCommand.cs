using BeersApi.Models.Output.Flavours;
using MediatR;

namespace BeersApi.Features.Flavours.Delete
{
   public class DeleteCommand : IRequest<Flavour>
   {
      public int FlavourId { get; set; }
   }
}
