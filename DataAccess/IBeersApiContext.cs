using Domain.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess
{
   public interface IBeersApiContext
   {
      DbSet<Beer> Beers { get; }

      DbSet<Category> Categories { get; }

      DbSet<Color> Colors { get; }

      DbSet<Country> Countries { get; }

      DbSet<Flavour> Flavours { get; }

      DbSet<Image> Images { get; }

      DbSet<User> Users { get; }


      int SaveChanges();

      Task<int> SaveChangesAsync(CancellationToken cancellationToken);

      Task<int> SaveChangesAsync();
   }
}
