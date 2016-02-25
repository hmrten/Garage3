namespace Garage3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Parkings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParkingSlotId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        DateIn = c.DateTime(nullable: false),
                        DateOut = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: true)
                .Index(t => t.VehicleId);
            
            CreateTable(
                "dbo.ParkingSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId)
                .Index(t => t.VehicleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParkingSlots", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Parkings", "VehicleId", "dbo.Vehicles");
            DropIndex("dbo.ParkingSlots", new[] { "VehicleId" });
            DropIndex("dbo.Parkings", new[] { "VehicleId" });
            DropTable("dbo.ParkingSlots");
            DropTable("dbo.Parkings");
        }
    }
}
