﻿using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class ProgrammingLanguageRepository : GenericRepository<ProgrammingLanguage, FriendOrganizerDbContext>, IProgrammingLanguageRepository
    {
        public ProgrammingLanguageRepository(FriendOrganizerDbContext context)
            : base(context)
        {
        }

        public async Task<bool> IsReferencedByFriendAsync(int programmingLanguageId)
        {
            return await _context.Friends.AsNoTracking()
                .AnyAsync(f => f.FavoriteLanguageId == programmingLanguageId);
        }
    }
}
