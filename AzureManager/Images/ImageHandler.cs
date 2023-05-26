using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureStorageManager.Images
{
   public class ImageHandler : IImageHandler
   {
      private readonly BlobServiceClient _blobServiceClient;

      public ImageHandler(BlobServiceClient blobServiceClient)
      {
         _blobServiceClient = blobServiceClient;
      }

      public async Task<string> UploadImage(string dataUri, string name, string containerName)
      {
         return await UploadBlobIntoContainer(dataUri, name, containerName).ConfigureAwait(false);
      }


      #region Private Methods

      private async Task<string> UploadBlobIntoContainer(string dataUri, string name, string containerName)
      {
         var position = dataUri.IndexOf(',');
         var data = dataUri.Substring(position + 1);
         var bytes = Convert.FromBase64String(data);
         await using var memoryStream = new MemoryStream(bytes);

         var blobContainer = _blobServiceClient.GetBlobContainerClient(containerName);
         var blobClient = blobContainer.GetBlobClient(name);

         var blobHttpHeaders = new BlobHttpHeaders
         {
            ContentType = "application/image"
         };
         await blobClient.UploadAsync(memoryStream, blobHttpHeaders).ConfigureAwait(false);

         return blobClient.Uri.AbsoluteUri;
      }

      #endregion
   }
}
