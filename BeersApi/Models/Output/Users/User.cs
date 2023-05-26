using AutoMapper;
using System;

namespace BeersApi.Models.Output.Users
{
   public class User
   {
      public Guid UId { get; set; }

      public int Id { get; set; }

      public string Email { get; set; }

      public string RoleName { get; set; }

      public string FirstName { get; set; }
   }

   public class UserMappingProfile : Profile
   {
      public UserMappingProfile()
      {
         CreateMap<Domain.Authorization.User, User>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.ToString()));
      }
   }
}
