namespace UrlShortener.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameToLink : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Urls", newName: "Links");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Links", newName: "Urls");
        }
    }
}
