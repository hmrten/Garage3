namespace Garage3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegNr = c.String(nullable: false, maxLength: 32),
                        OwnerId = c.Int(nullable: false),
                        VehicleTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Owners", t => t.OwnerId, cascadeDelete: true)
                .ForeignKey("dbo.VehicleTypes", t => t.VehicleTypeId, cascadeDelete: true)
                .Index(t => t.OwnerId)
                .Index(t => t.VehicleTypeId);
            
            CreateTable(
                "dbo.VehicleTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        ParkingId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parkings", t => t.ParkingId)
                .Index(t => t.ParkingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ParkingSlots", "ParkingId", "dbo.Parkings");
            DropForeignKey("dbo.Parkings", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "VehicleTypeId", "dbo.VehicleTypes");
            DropForeignKey("dbo.Vehicles", "OwnerId", "dbo.Owners");
            DropIndex("dbo.ParkingSlots", new[] { "ParkingId" });
            DropIndex("dbo.Parkings", new[] { "VehicleId" });
            DropIndex("dbo.Vehicles", new[] { "VehicleTypeId" });
            DropIndex("dbo.Vehicles", new[] { "OwnerId" });
            DropTable("dbo.ParkingSlots");
            DropTable("dbo.Parkings");
            DropTable("dbo.VehicleTypes");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Owners");
        }
    }
}
