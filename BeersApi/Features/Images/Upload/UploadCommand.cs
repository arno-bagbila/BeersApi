using BeersApi.Models.Input.Images;
using MediatR;

namespace BeersApi.Features.Images.Upload
{
   public class UploadCommand : IRequest<string>
   {
      public UploadImage UploadImage { get; set; }
   }
}
