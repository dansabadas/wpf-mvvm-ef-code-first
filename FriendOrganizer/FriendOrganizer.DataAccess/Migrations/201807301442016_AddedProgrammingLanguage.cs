namespace FriendOrganizer.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddedProgrammingLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "wpfmvvm.ProgrammingLanguage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("wpfmvvm.Friend", "FavoriteLanguageId", c => c.Int());
            CreateIndex("wpfmvvm.Friend", "FavoriteLanguageId");
            AddForeignKey("wpfmvvm.Friend", "FavoriteLanguageId", "wpfmvvm.ProgrammingLanguage", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("wpfmvvm.Friend", "FavoriteLanguageId", "wpfmvvm.ProgrammingLanguage");
            DropIndex("wpfmvvm.Friend", new[] { "FavoriteLanguageId" });
            DropColumn("wpfmvvm.Friend", "FavoriteLanguageId");
            DropTable("wpfmvvm.ProgrammingLanguage");
        }
    }
}
