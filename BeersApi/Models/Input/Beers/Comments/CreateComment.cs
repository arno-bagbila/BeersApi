using FluentValidation;
using System;

namespace BeersApi.Models.Input.Beers.Comments
{
   public class CreateComment
   {
      /// <summary>
      /// Comment text
      /// </summary>
      public string Body { get; set; }

      /// <summary>
      /// username of user who set the comment
      /// </summary>
      public string UserFirstName { get; set; }

      /// <summary>
      /// User Id
      /// </summary>
      public Guid UserUId { get; set; }

      /// <summary>
      /// Commented beer id
      /// </summary>
      public int BeerId { get; set; }
   }

   public class CreateCommentValidator : AbstractValidator<CreateComment>
   {
      public CreateCommentValidator()
      {
         RuleFor(c => c.Body)
            .NotEmpty()
            .MaximumLength(3000);

         RuleFor(c => c.UserFirstName)
            .NotEmpty()
            .MaximumLength(256);

         RuleFor(c => c.BeerId)
            .GreaterThan(0)
            .NotEmpty();

         RuleFor(c => c.UserUId)
            .NotEmpty();
      }
   }
}
