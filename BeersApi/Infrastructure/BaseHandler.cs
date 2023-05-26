using DataAccess;

namespace BeersApi.Infrastructure
{
   public abstract class BaseHandler
   {
      #region Data

      protected IBeersApiContext Ctx;

      #endregion

      #region Constructors

      protected BaseHandler(IBeersApiContext ctx)
      {
         Ctx = ctx;
      }

      #endregion
   }
}
