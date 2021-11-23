using SmollApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmollApi.Repositories
{
    public interface IFavouriteRepository
    {
        Task<Favourite> addToFavourite(Favourite favourite, int UserID, int PhoneID);
        Task<Favourite> checkFav(int id);

        Task remove(Favourite fav);
    }
    public class FavouriteRepository : IFavouriteRepository
    {
        private readonly EshopDBContext _context;

        public FavouriteRepository(EshopDBContext context)
        {
            _context = context;
        }

        public async Task<Favourite> checkFav(int id)
        {
            return await _context.Favourites.FindAsync(id);
        }

        public async Task<Favourite> addToFavourite(Favourite favourite, int UserID, int PhoneID)
        {
            favourite.PhonesId = PhoneID;
            favourite.UserId = UserID;

            //if (checkFav(UserID) == null)
            //    return favourite;

            _context.Favourites.Add(favourite);

            await _context.SaveChangesAsync();

            return favourite;
        }

        public async Task remove(Favourite fav)
        {
            var favToDelete = await _context.Favourites.FindAsync(fav.PhonesId);
            _context.Favourites.Remove(favToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
