namespace Garage3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Vehicles", "RegNr", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Vehicles", new[] { "RegNr" });
        }
    }
}
