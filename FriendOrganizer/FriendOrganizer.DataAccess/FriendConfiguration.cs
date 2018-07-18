using System.Data.Entity.ModelConfiguration;
using FriendOrganizer.Model;

namespace FriendOrganizer.DataAccess
{
    public class FriendConfiguration : EntityTypeConfiguration<Friend>
    {
        public FriendConfiguration()
        {
            Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
