using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data
{
    public class FriendDataService : IFriendDataService
    {
        //public IEnumerable<Friend> GetAll()
        //{
        //    yield return new Friend { FirstName = "Thomas", LastName = "Huber" };
        //    yield return new Friend { FirstName = "Dan", LastName = "Sabadis" };
        //    yield return new Friend { FirstName = "Julia", LastName = "Huber" };
        //    yield return new Friend { FirstName = "Chrissi", LastName = "Egin" };
        //}

        private Func<FriendOrganizerDbContext> _contextCreator;

        public FriendDataService(Func<FriendOrganizerDbContext> contextCreator)
        {
            _contextCreator = contextCreator;
        }
        public async Task<List<Friend>> GetAllAsync()
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Friends.AsNoTracking().ToListAsync();
            }
        }
    }
}
