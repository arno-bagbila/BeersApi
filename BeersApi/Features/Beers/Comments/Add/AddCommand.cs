using BeersApi.Models.Input.Beers.Comments;
using BeersApi.Models.Output.Beers;
using MediatR;

namespace BeersApi.Features.Beers.Comments.Add
{
   public class AddCommand : IRequest<Beer>
   {
      public CreateComment Comment { get; set; }
   }
}
