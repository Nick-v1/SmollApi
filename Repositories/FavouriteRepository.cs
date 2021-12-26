using Microsoft.EntityFrameworkCore;
using SmollApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IFavouriteRepository
    {
        Task<Favourite> addToFavourite(Favourite favourite);
        Task<Favourite> checkFav(int id);
        Task<IEnumerable<Favourite>> Get();
        Task<IEnumerable<Favourite>> Get(int userId);
        Task remove(Favourite fav);
    }
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly EshopDBContext _context;

        public FavouriteRepository(EshopDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favourite>> Get()
        {
            return await _context.Favourites.ToListAsync();
        }
        public async Task<Favourite> checkFav(int id)
        {
            return await _context.Favourites.FindAsync(id);
        }


        public async Task<Favourite> addToFavourite(Favourite favourite)
        {
            _context.Favourites.Add(favourite);

            await _context.SaveChangesAsync();

            return favourite;
        }

        public async Task remove(Favourite fav)
        {
            _context.Favourites.Remove(fav);

            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Favourite>> Get(int userId)
        {
            return (await _context.Favourites.ToListAsync()).Where(i => i.UserId == userId);
        } // returns a list filtered by userid;
    }
}
