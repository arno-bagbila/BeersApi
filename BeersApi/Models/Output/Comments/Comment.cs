using AutoMapper;
using System;

namespace BeersApi.Models.Output.Comments
{
   /// <summary>
   /// Output model for <see cref="Domain.Entities.Comment"/>
   /// </summary>
   public class Comment
   {
      /// <summary>
      /// Comment body
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// User first name
      /// </summary>
      public string UserFirstName { get; set; }

      /// <summary>
      /// Time when the comment was posted
      /// </summary>
      public DateTime DatePosted { get; set; }
   }

   public class CommentMappingProfile : Profile
   {
      public CommentMappingProfile()
      {
         CreateMap<Domain.Entities.Comment, Comment>();
      }
   }
}
