using BeersApi.Features.Users.Create;
using BeersApi.Features.Users.Search;
using BeersApi.Infrastructure;
using BeersApi.Models.Input.Users.Create;
using BeersApi.Models.Output.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BeersApi.Features.Users
{
   [Route("[controller]")]
   public class UsersController : MediatorAwareController
   {
      #region Constructors

      public UsersController(IMediator mediator) : base(mediator)
      {
      }

      #endregion

      #region Apis

      /// <summary>
      /// Get User by Email
      /// </summary>
      /// <param name="email">User email</param>
      /// <param name="cancellationToken">A cancellation token for this request</param>
      /// <returns>A unique user <see cref="User"/> with the email address</returns>
      [HttpGet("{email}", Name = "UserByEmail")]
      [Authorize]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
      public async Task<IActionResult> Get(string email, CancellationToken cancellationToken)
      {
         var searchQuery = new SearchQuery { Email = email };
         var results = await Mediator.Send(searchQuery, cancellationToken);
         var result = results.FirstOrDefault();

         return result is null ? NotFound() : Ok(result);
      }

      /// <summary>
      /// Create a <see cref="User"/>
      /// </summary>
      /// <param name="model">values of the <see cref="User"/> to create</param>
      /// <param name="cancellationToken">A cancellation token</param>
      /// <returns>a newly created <see cref="User"/></returns>
      [HttpPost]
      [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
      public async Task<IActionResult> Create([FromBody][Required] CreateUser model,
         CancellationToken cancellationToken)
      {
         var command = new CreateCommand
         {
            CreateUser = model
         };

         var result = await Mediator.Send(command, cancellationToken).ConfigureAwait(false);
         var location = new Uri(Url.Link("UserByEmail", new { result.Email }));
         return Created(location, result);
      }

      #endregion
   }
}