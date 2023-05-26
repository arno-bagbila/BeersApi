using System;

namespace Domain.Entities
{
   public class EntityBaseDescription : EntityBase
   {
      public string Description { get; set; }

      public EntityBaseDescription(string name, string description) : base(name)
      {
         if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException($"{nameof(description)} cannot be null");
         if (description.Length < 3)
            throw new ArgumentException($"{nameof(description)} must be greater than 3");

         Description = description;
      }

   }
}
