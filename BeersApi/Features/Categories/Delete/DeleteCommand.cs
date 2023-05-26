using BeersApi.Models.Output.Categories;
using MediatR;

namespace BeersApi.Features.Categories.Delete
{
   public class DeleteCommand : IRequest<Category>
   {
      public int CategoryId { get; set; }
   }
}
