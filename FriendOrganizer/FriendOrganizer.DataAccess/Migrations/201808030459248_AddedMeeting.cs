namespace FriendOrganizer.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMeeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "wpfmvvm.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "wpfmvvm.MeetingFriend",
                c => new
                    {
                        Meeting_Id = c.Int(nullable: false),
                        Friend_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Meeting_Id, t.Friend_Id })
                .ForeignKey("wpfmvvm.Meeting", t => t.Meeting_Id, cascadeDelete: true)
                .ForeignKey("wpfmvvm.Friend", t => t.Friend_Id, cascadeDelete: true)
                .Index(t => t.Meeting_Id)
                .Index(t => t.Friend_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("wpfmvvm.MeetingFriend", "Friend_Id", "wpfmvvm.Friend");
            DropForeignKey("wpfmvvm.MeetingFriend", "Meeting_Id", "wpfmvvm.Meeting");
            DropIndex("wpfmvvm.MeetingFriend", new[] { "Friend_Id" });
            DropIndex("wpfmvvm.MeetingFriend", new[] { "Meeting_Id" });
            DropTable("wpfmvvm.MeetingFriend");
            DropTable("wpfmvvm.Meeting");
        }
    }
}
