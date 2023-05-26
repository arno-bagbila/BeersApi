using BeersApi.Models.Input.Flavours.Update;
using BeersApi.Models.Output.Flavours;
using MediatR;

namespace BeersApi.Features.Flavours.Update
{
   public class UpdateCommand : IRequest<Flavour>
   {
      public int FlavourId { get; set; }

      public UpdateFlavour UpdateFlavour { get; set; }
   }
}
