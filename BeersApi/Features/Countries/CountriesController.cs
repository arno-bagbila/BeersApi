using BeersApi.ActionFilters;
using BeersApi.Features.Countries.Search;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Countries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Countries
{
   [Route("[controller]")]
   public class CountriesController : MediatorAwareController
   {

      #region Constructors

      public CountriesController(IMediator mediator) : base(mediator) { }

      #endregion

      #region Apis

      /// <summary>
      /// Get all the <see cref="Country"/>
      /// </summary>
      /// <param name="cancellationToken">A <see cref="CancellationToken"/> for the request</param>
      /// <returns>List of <see cref="Country"/></returns>
      [HttpGet(Name = "CountryList")]
      [ProducesResponseType(typeof(IEnumerable<Country>), StatusCodes.Status200OK)]
      public async Task<IActionResult> List(CancellationToken cancellationToken) =>
         Ok(await Mediator.Send(new SearchQuery(), cancellationToken).ConfigureAwait(false));

      /// <summary>
      /// Get specific <see cref="Country"/> by its Id
      /// </summary>
      /// <param name="id"><see cref="Country"/> id</param>
      /// <param name="cancellationToken"> A <see cref="CancellationToken"/> for the request</param>
      /// <returns>Get specific <see cref="Country"/></returns>
      [HttpGet("{id:int}", Name = "CountryById")]
      [ProducesResponseType(404)]
      [ProducesResponseType(200)]
      [ValidateEntityId]
      public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
      {
         var query = new SearchQuery
         {
            CountryId = id
         };

         var results = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
         var result = results.FirstOrDefault();

         return result is null ? NotFound() : (IActionResult)Ok(result);
      }

      #endregion
   }
}