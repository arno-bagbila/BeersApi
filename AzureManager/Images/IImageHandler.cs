using System.Threading.Tasks;

namespace AzureStorageManager.Images
{
   public interface IImageHandler
   {
      Task<string> UploadImage(string dataUri, string name, string containerName);
   }
}
