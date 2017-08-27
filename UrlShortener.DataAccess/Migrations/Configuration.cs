namespace UrlShortener.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UrlShortenerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
			AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(UrlShortenerContext context)
        {
        }
    }
}
