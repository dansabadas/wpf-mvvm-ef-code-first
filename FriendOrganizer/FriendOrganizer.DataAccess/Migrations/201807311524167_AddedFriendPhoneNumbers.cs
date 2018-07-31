namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFriendPhoneNumbers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "wpfmvvm.FriendPhoneNumber",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(nullable: false),
                        FriendId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("wpfmvvm.Friend", t => t.FriendId, cascadeDelete: true)
                .Index(t => t.FriendId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("wpfmvvm.FriendPhoneNumber", "FriendId", "wpfmvvm.Friend");
            DropIndex("wpfmvvm.FriendPhoneNumber", new[] { "FriendId" });
            DropTable("wpfmvvm.FriendPhoneNumber");
        }
    }
}
