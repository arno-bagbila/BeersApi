using BeersApi.ActionFilters;
using BeersApi.Features.Categories.Create;
using BeersApi.Features.Categories.Delete;
using BeersApi.Features.Categories.Search;
using BeersApi.Features.Categories.Update;
using BeersApi.Infrastructure;
using BeersApi.Models.Output.Categories;
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

namespace BeersApi.Features.Categories
{
   [Route("[controller]")]
   public class CategoriesController : MediatorAwareController
   {
      #region Constructors

      public CategoriesController(IMediator mediator) : base(mediator) { }

      #endregion

      #region Apis

      /// <summary>
      /// Get all categories
      /// </summary>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns>A list of <see cref="Category"/></returns>
      [HttpGet(Name = "CategoryList")]
      [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
      public async Task<IActionResult> List(CancellationToken cancellationToken)
      {
         var result = await Mediator.Send(new SearchQuery(), cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Get category by Id
      /// </summary>
      /// <param name="id">Id of the specific category</param>
      /// <param name="cancellationToken">A cancellation token for this request</param>
      /// <returns>A specific <see cref="Category"/></returns>
      [HttpGet("{id}", Name = "CategoryById")]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ValidateEntityId]
      public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
      {
         var searchQuery = new SearchQuery { Id = id };
         var results = await Mediator.Send(searchQuery, cancellationToken);
         var result = results.FirstOrDefault();

         return result is null ? NotFound() : Ok(result);
      }

      /// <summary>
      /// Create an <see cref="Category"/>
      /// </summary>
      /// <param name="model">Values of the <see cref="Category"/> to create</param>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns></returns>
      [HttpPost]
      [ServiceFilter(typeof(ValidationFilterAttribute))]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      public async Task<IActionResult> Create([FromBody][Required] Models.Input.Categories.Create.CreateCategory model,
         CancellationToken cancellationToken)
      {
         var command = new CreateCommand
         {
            CreateCategory = model
         };

         var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
         var location = new Uri(Url.Link("CategoryById", new { result.Id }));
         return Created(location, result);
      }

      /// <summary>
      /// Update <see cref="Category"/>
      /// </summary>
      /// <param name="id">Id of the <see cref="Category"/> to update</param>
      /// <param name="model">Updated values for the <see cref="Category"/></param>
      /// <param name="cancellationToken"></param>
      /// <returns>The updated <see cref="Category"/></returns>
      [HttpPut("{id}")]
      [ValidateEntityId]
      [ServiceFilter(typeof(ValidationFilterAttribute))]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> Update(int id,
         [FromBody][Required] Models.Input.Categories.Update.UpdateCategory model, CancellationToken cancellationToken)
      {
         var command = new UpdateCommand
         {
            UpdateCategory = model,
            CategoryId = id
         };

         var result = await Mediator.Send(command, cancellationToken);

         return Ok(result);
      }

      /// <summary>
      /// Delete <see cref="Category"/>
      /// </summary>
      /// <param name="id">Id of the <see cref="Category"/> to delete</param>
      /// <param name="cancellationToken">A cancellation token for the request</param>
      /// <returns></returns>
      [HttpDelete("{id}")]
      [ValidateEntityId]
      [Authorize(Policy = "BeersApiRole")]
      [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status403Forbidden)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
      {
         var command = new DeleteCommand
         {
            CategoryId = id
         };

         var result = await Mediator.Send(command, cancellationToken);

         return Ok(result);
      }

      #endregion
   }
}