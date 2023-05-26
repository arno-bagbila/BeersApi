using System;

namespace Domain.Entities
{
   public class EntityBase
   {
      public Guid Id { get; private set; }
      public string Name { get; private set; }

      public EntityBase() { }

      public EntityBase(string name)
      {
         if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException($"{nameof(name)} cannot be null");
         if (name.Length > 50)
            throw new ArgumentException($"{nameof(name)} cannot be longer than 50 characters");
         Name = name;
         Id = Guid.NewGuid();
      }
   }
}
