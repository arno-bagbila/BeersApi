using BeersApi.ActionFilters;
using BeersApi.Features.Beers.Comments.Add;
using BeersApi.Features.Beers.Create;
using BeersApi.Features.Beers.Delete;
using BeersApi.Features.Beers.Search;
using BeersApi.Features.Beers.Update;
using BeersApi.Infrastructure;
using BeersApi.Models.Input.Beers.Comments;
using BeersApi.Models.Input.Beers.Create;
using BeersApi.Models.Output.Beers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Beers
{
   [Route("[controller]")]
   public class BeersController : MediatorAwareController
   {

      public BeersController(IMediator mediator) : base(mediator) { }

      /// <summary>
      /// Get all <see cref="Beer"/>
      /// </summary>
      /// <param name="beerFilter">The beer filter</param>
      /// <param name="cancellationToken">A <see cref="CancellationToken"/> for the request</param>
      /// <returns>A list of <see cref="Beer"/></returns>
      [HttpGet]
      [ProducesResponseType(typeof(IEnumerable<Beer>), StatusCodes.Status200OK)]
      public async Task<IActionResult> List([FromQuery] BeersFilter beerFilter, CancellationToken cancellationToken) =>
         Ok(await Mediator.Send(new SearchQuery { BeersFilter = beerFilter }, cancellationToken).ConfigureAwait(false));


      /// <summary>
      /// Get Beer by Id
      /// </summary>
      /// <param name="id">The id used to retrieve the <see cref="Beer"/></param>
      /// <param name="cancellationToken">A cancellation token for this request</param>
      /// <returns>Specific <see cref="Beer"/></returns>
      /// <response code = "404">The beer was not found</response>
      /// <response code="200">Returns beer with the given Id</response>
      //[Authorize(AuthenticationSchemes = "JwtBearer", Policy = "CanDoEverything")]
      [HttpGet("{id:int}", Name = "BeerById")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
      [ValidateEntityId]
      public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
      {
         var query = new SearchQuery { Id = id };
         var results = await Mediator.Send(query, cancellationToken);
         var result = results.FirstOrDefault();

         return result is null ? NotFound() : Ok(result);
      }

      /// <summary>
      /// Register a new <see cref="Beer"/>
      /// </summary>
      /// <param name="model">Values of the <see cref="Domain.Entities.Beer"/> to register</param>
      /// <param name="cancellationToken">A <see cref="CancellationToken"/> for the request</param>
      /// <returns>the registered <see cref="Beer"/></returns>
      [HttpPost]
      [ServiceFilter(typeof(ValidationFilterAttribute))]
      [ServiceFilter(typeof(ValidatorActionFilter))]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Beer), StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      public async Task<IActionResult> Create([FromBody][Required] CreateBeer model,
          CancellationToken cancellationToken)
      {
         var command = new CreateCommand
         {
            Beer = model
         };
         var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);

         return CreatedAtRoute("BeerById", new { id = result.Id }, result);
      }

      /// <summary>
      /// Update <see cref="Beer"/>
      /// </summary>
      /// <param name="id">Id of the <see cref="Beer"/> to update</param>
      /// <param name="model">Updated values for the <see cref="Domain.Entities.Beer"/></param>
      /// <param name="cancellationToken"></param>
      /// <returns>The updated <see cref="Beer"/></returns>
      [HttpPut("{id:int}")]
      [ValidateEntityId]
      [ServiceFilter(typeof(ValidationFilterAttribute))]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      public async Task<IActionResult> Update(int id,
         [FromBody][Required] Models.Input.Beers.Update.UpdateBeer model, CancellationToken cancellationToken)
      {
         var command = new UpdateCommand
         {
            Beer = model,
            BeerId = id
         };

         var result = await Mediator.Send(command, cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Delete <see cref="Beer"/>
      /// </summary>
      /// <param name="id">Id of the <see cref="Beer"/> to delete</param>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns>The deleted <see cref="Beer"/></returns>
      [HttpDelete("{id:int}")]
      [ValidateEntityId]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
      public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
      {
         var command = new DeleteCommand
         {
            BeerId = id
         };

         var result = await Mediator.Send(command, cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Add a comment to a <see cref="Beer"/>
      /// </summary>
      /// <param name="createComment">Comment to add</param>
      /// <param name="cancellationToken"></param>
      /// <returns><see cref="Beer"/> with the added comment</returns>
      [HttpPost("comment")]
      [Authorize]
      [ProducesResponseType(typeof(Beer), StatusCodes.Status200OK)]
      public async Task<IActionResult> AddComment([FromBody][Required] CreateComment createComment,
         CancellationToken cancellationToken)
      {
         var addCommand = new AddCommand
         {
            Comment = createComment
         };

         var result = await Mediator.Send(addCommand, cancellationToken);

         return Ok(result);
      }
   }
}