using Autofac;
using DataAccess;

namespace BeersApi.Infrastructure.IoC.Installers
{
   public class DataAccessInstaller : Module
   {
      public class InfrastructureInstaller : Module
      {

         protected override void Load(ContainerBuilder builder)
         {

            builder
               .RegisterAssemblyTypes(typeof(IBeersApiContext).Assembly)
               .AsImplementedInterfaces();
         }
      }
   }
}
