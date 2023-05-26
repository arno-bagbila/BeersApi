using BeersApi.Models.Input.Users.Create;
using BeersApi.Models.Output.Users;
using MediatR;

namespace BeersApi.Features.Users.Create
{
   public class CreateCommand : IRequest<User>
   {
      public CreateUser CreateUser { get; set; }
   }
}
