using BeersApi.ActionFilters;
using BeersApi.Features.Colors.Search;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Colors;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Colors
{
   [Route("[controller]")]
   public class ColorsController : MediatorAwareController
   {

      #region Constructors

      public ColorsController(IMediator mediator) : base(mediator)
      {
      }

      #endregion

      #region Apis

      /// <summary>
      /// Get specific <see cref="Color"/> by its id
      /// </summary>
      /// <param name="id">Id of the <see cref="Color"/></param>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns>Specific <see cref="Color"/></returns>
      [HttpGet("{id:int}", Name = "ColorById")]
      [ProducesResponseType(404)]
      [ProducesResponseType(200)]
      [ValidateEntityId]
      public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
      {
         var searchQuery = new SearchQuery
         {
            ColorId = id
         };
         var results = await Mediator.Send(searchQuery, cancellationToken);
         var result = results.FirstOrDefault();

         return result is null ? NotFound() : Ok(result);
      }


      /// <summary>
      /// Get all the <see cref="Color"/>
      /// </summary>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns>All <see cref="Color"/></returns>
      [HttpGet(Name = "ColorList")]
      [ProducesResponseType(typeof(IEnumerable<Color>), StatusCodes.Status200OK)]
      public async Task<IActionResult> List(CancellationToken cancellationToken) =>
         Ok(await Mediator.Send(new SearchQuery(), cancellationToken).ConfigureAwait(false));

      #endregion

   }
}