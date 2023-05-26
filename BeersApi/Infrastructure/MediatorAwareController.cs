using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BeersApi.Infrastructure
{
   public abstract class MediatorAwareController : ControllerBase
   {

      protected readonly IMediator Mediator;

      #region Cosnstructors

      protected MediatorAwareController(IMediator mediator)
      {
         Mediator = mediator;
      }

      #endregion
   }
}
