using AzureStorageManager.Images;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Images.Upload
{
   public class UploadCommandHandler : IRequestHandler<UploadCommand, string>
   {
      private readonly IImageHandler _imageHandler;

      public UploadCommandHandler(IImageHandler imageHandler)
      {
         _imageHandler = imageHandler;
      }

      public async Task<string> Handle(UploadCommand command, CancellationToken cancellationToken) =>
         await _imageHandler
            .UploadImage(command.UploadImage.DataUri, command.UploadImage.Name, "beersapilogourls")
            .ConfigureAwait(false);
   }
}
