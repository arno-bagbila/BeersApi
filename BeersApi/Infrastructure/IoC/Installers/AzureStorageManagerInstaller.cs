using Autofac;
using AzureStorageManager.Images;

namespace BeersApi.Infrastructure.IoC.Installers
{
   public class AzureStorageManagerInstaller : Module
   {
      protected override void Load(ContainerBuilder builder)
      {

         builder
            .RegisterAssemblyTypes(typeof(IImageHandler).Assembly)
            .AsImplementedInterfaces();
      }
   }
}
