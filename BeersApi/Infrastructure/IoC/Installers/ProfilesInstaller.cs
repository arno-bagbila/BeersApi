using Autofac;
using AutoMapper;

namespace BeersApi.Infrastructure.IoC.Installers
{
   /// <summary>
   /// Register Automapper profiles
   /// </summary>
   public class ProfilesInstaller : Module
   {
      protected override void Load(ContainerBuilder builder)
      {

         var mapper = new MapperConfiguration(cfg =>
         {

            cfg.AddMaps(ThisAssembly);
            //cfg.ConstructServicesUsing(t => container.Resolve(t));
         }).CreateMapper();

         builder
            .RegisterInstance(mapper)
            .AsImplementedInterfaces();
      }
   }
}
