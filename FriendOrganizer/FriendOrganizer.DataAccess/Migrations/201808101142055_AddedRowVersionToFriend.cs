namespace FriendOrganizer.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedRowVersionToFriend : DbMigration
    {
        public override void Up()
        {
            AddColumn("wpfmvvm.Friend", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("wpfmvvm.Friend", "RowVersion");
        }
    }
}
