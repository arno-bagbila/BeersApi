using FluentValidation;

namespace BeersApi.Models.Input.Images
{
   public class UploadImage
   {
      /// <summary>
      /// Image data uri we get from the frontend
      /// </summary>
      public string DataUri { get; set; }

      /// <summary>
      /// Image name to be set in Azure storage blob container
      /// </summary>
      public string Name { get; set; }
   }

   public class UploadImageValidator : AbstractValidator<UploadImage>
   {

   }
}
