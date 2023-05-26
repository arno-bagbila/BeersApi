using Autofac;
using MediatR;
using System.Reflection;
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

         builder.Register<ServiceFactory>(ctx =>
         {

            var c = ctx.Resolve<IComponentContext>();
            return t => c.Resolve(t);
         });
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
