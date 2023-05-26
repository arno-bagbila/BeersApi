using BeersApi.Models.Input.Flavours.Create;
using BeersApi.Models.Output.Flavours;
using MediatR;

namespace BeersApi.Features.Flavours.Create
{
   public class CreateCommand : IRequest<Flavour>
   {
      public CreateFlavour CreateFlavour { get; set; }
   }
}
