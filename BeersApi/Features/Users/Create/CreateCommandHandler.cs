using AutoMapper;
using BeersApi.Infrastructure;
using DataAccess;
using Domain;
using Domain.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using User = BeersApi.Models.Output.Users.User;

namespace BeersApi.Features.Users.Create
{
   public class CreateCommandHandler : BaseHandler, IRequestHandler<CreateCommand, User>
   {
      private readonly IMapper _mapper;

      public CreateCommandHandler(IBeersApiContext ctx, IMapper mapper) : base(ctx) => _mapper = mapper;

      public async Task<User> Handle(CreateCommand request, CancellationToken cancellationToken)
      {
         var createUser = request.CreateUser;
         if (!Enum.TryParse(createUser.RoleName, true, out Role role))
            throw BeersApiException.Create(BeersApiException.InvalidDataCode,
               $"{createUser.RoleName} is not a valid role");

         var existingUserWithSameEmail =
            await Ctx.Users.FirstOrDefaultAsync(u => u.Email == createUser.Email, cancellationToken).ConfigureAwait(false);
         if (existingUserWithSameEmail != null)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode,
               $"A user with email {createUser.Email} already exists.");

         var existingUserWithSameUId =
            await Ctx.Users.FirstOrDefaultAsync(u => u.UId == createUser.UId, cancellationToken).ConfigureAwait(false);
         if (existingUserWithSameUId != null)
            throw BeersApiException.Create(BeersApiException.InvalidDataCode,
               $"A user with UId {createUser.UId} already exists.");

         var user = Domain.Authorization.User.Create(createUser.UId, createUser.Email, role, createUser.FirstName);
         await Ctx.Users.AddAsync(user, cancellationToken).ConfigureAwait(false);
         await Ctx.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

         return _mapper.Map<User>(user);
      }
   }
}
