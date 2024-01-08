using Autofac;
using MediatR;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;

namespace BeersApi.Infrastructure.IoC.Installers
{
   public class MediatorInstaller : Module
   {
      #region Overrides

      protected override void Load(ContainerBuilder builder)
      {

         builder
            .RegisterAssemblyTypes(typeof(IMediator).Assembly)
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

         RegisterHandlers(builder);

         var services = new ServiceCollection();
         builder.Populate(services);
      }

      #endregion

      #region Internals

      private void RegisterHandlers(ContainerBuilder builder)
      {

         var mediatrOpenTypes = new[] {
            typeof(IRequestHandler<,>),
            typeof(INotificationHandler<>),
         };

         foreach (var mediatrOpenType in mediatrOpenTypes)
         {
            builder
               .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .AsClosedTypesOf(mediatrOpenType)
               .AsImplementedInterfaces()
               .InstancePerLifetimeScope();
         }
      }

      #endregion
   }
}
