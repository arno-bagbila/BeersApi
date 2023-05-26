using BeersApi.Features.Images.Upload;
using BeersApi.Infrastructure;
using BeersApi.Models.Input.Images;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Images
{
   [Route("[controller]")]
   public class ImagesController : MediatorAwareController
   {
      public ImagesController(IMediator mediator) : base(mediator) { }

      [HttpPost]
      public async Task<IActionResult> Upload([FromBody][Required] UploadImage model,
         CancellationToken cancellationToken)
      {
         var command = new UploadCommand
         {
            UploadImage = model
         };

         var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

         return Ok(new { url = result });
      }
   }
}
