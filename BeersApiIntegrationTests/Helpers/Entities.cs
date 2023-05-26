using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeersApi.IntegrationTests.Helpers
{
   public static class Entities
   {
      public static async Task<Guid> GetColorId()
      {
         DbContextFactory contextFactory = new DbContextFactory();
         var beersApiContext = contextFactory.Context;
         var color = Domain.Entities.Color.Create("color name");
         await beersApiContext.AddAsync((color));
         await beersApiContext.SaveChangesAsync();

         return color.UId;
      }

      public static async Task<Guid> GetCategoryId()
      {
         DbContextFactory contextFactory = new DbContextFactory();
         var beersApiContext = contextFactory.Context;
         var category = Domain.Entities.Category.Create("category name", "category description");
         await beersApiContext.Categories.AddAsync(category);
         await beersApiContext.SaveChangesAsync();

         return category.UId;
      }
   }
}
