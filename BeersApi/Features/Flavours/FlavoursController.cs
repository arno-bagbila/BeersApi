using BeersApi.ActionFilters;
using BeersApi.Features.Flavours.Create;
using BeersApi.Features.Flavours.Delete;
using BeersApi.Features.Flavours.Search;
using BeersApi.Features.Flavours.Update;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Flavours;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Flavours
{
   [Route("[controller]")]
   public class FlavoursController : MediatorAwareController
   {

      #region Constructors

      public FlavoursController(IMediator mediator) : base(mediator) { }

      #endregion

      #region Apis

      /// <summary>
      /// Get all the <see cref="Flavour"/>
      /// </summary>
      /// <param name="cancellationToken">A <see cref="CancellationToken"/> for the request</param>
      /// <returns>List of <see cref="Flavour"/></returns>
      [HttpGet(Name = "FlavourList")]
      [ProducesResponseType(typeof(IEnumerable<Flavour>), StatusCodes.Status200OK)]
      public async Task<IActionResult> List(CancellationToken cancellationToken) =>
         Ok(await Mediator.Send(new SearchQuery(), cancellationToken).ConfigureAwait(false));

      /// <summary>
      /// Get specific <see cref="Flavour"/> by its Id
      /// </summary>
      /// <param name="id"><see cref="Flavour"/> id</param>
      /// <param name="cancellationToken"> A <see cref="CancellationToken"/> for the request</param>
      /// <returns>Get specific <see cref="Flavour"/></returns>
      [HttpGet("{id}", Name = "FlavourById")]
      [ProducesResponseType(404)]
      [ProducesResponseType(200)]
      [ValidateEntityId]
      public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
      {
         var query = new SearchQuery
         {
            FlavourId = id
         };

         var results = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);
         var result = results.FirstOrDefault();

         return result is null ? NotFound() : (IActionResult)Ok(result);
      }

      /// <summary>
      /// Create an <see cref="Flavour"/>
      /// </summary>
      /// <param name="model">Values of the <see cref="Models.Output.Flavours.Flavour"/> to create</param>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns>A <see cref="Flavour"/></returns>
      [HttpPost]
      [Authorize(Policy = "BeersApiRole")]
      [ServiceFilter(typeof(ValidationFilterAttribute))]
      [ProducesResponseType(typeof(Flavour), StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      public async Task<IActionResult> Create([FromBody][Required] Models.Input.Flavours.Create.CreateFlavour model,
         CancellationToken cancellationToken)
      {
         var command = new CreateCommand
         {
            CreateFlavour = model
         };

         var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
         var location = new Uri(Url.Link("FlavourById", new { result.Id }));

         return Created(location, result);
      }

      /// <summary>
      /// Update <see cref="Flavour"/>
      /// </summary>
      /// <param name="id">Id of the <see cref="Flavour"/> to update</param>
      /// <param name="model">Updated values for the <see cref="Flavour"/></param>
      /// <param name="cancellationToken"></param>
      /// <returns>The updated <see cref="Flavour"/></returns>
      [HttpPut("{id}")]
      [Authorize(Policy = "BeersApiRole")]
      [ValidateEntityId]
      [ServiceFilter(typeof(ValidationFilterAttribute))]
      [ProducesResponseType(typeof(Flavour), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      public async Task<IActionResult> Update(int id,
         [FromBody][Required] Models.Input.Flavours.Update.UpdateFlavour model, CancellationToken cancellationToken)
      {
         var command = new UpdateCommand
         {
            FlavourId = id,
            UpdateFlavour = model
         };

         var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

         return Ok(result);
      }

      /// <summary>
      /// Delete <see cref="Flavour"/>
      /// </summary>
      /// <param name="id">Id of the <see cref="Flavour"/> to delete</param>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns></returns>
      [HttpDelete("{id:int}")]
      [ValidateEntityId]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Flavour), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
      {
         var command = new DeleteCommand
         {
            FlavourId = id
         };

         var result = await Mediator.Send(command, cancellationToken);

         return Ok(result);
      }

      #endregion
   }
}