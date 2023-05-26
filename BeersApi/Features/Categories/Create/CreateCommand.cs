using BeersApi.Models.Input.Categories.Create;
using BeersApi.Models.Output.Categories;
using MediatR;

namespace BeersApi.Features.Categories.Create
{
   public class CreateCommand : IRequest<Category>
   {
      public CreateCategory CreateCategory { get; set; }
   }
}
