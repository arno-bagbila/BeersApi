using BeersApi.Models.Output.Categories;
using MediatR;

namespace BeersApi.Features.Categories.Update
{
   public class UpdateCommand : IRequest<Category>
   {
      public int CategoryId { get; set; }
      public Models.Input.Categories.Update.UpdateCategory UpdateCategory { get; set; }
   }
}
