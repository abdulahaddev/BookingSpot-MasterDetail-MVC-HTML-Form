namespace MVC_EV_FINAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class script : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingEntries",
                c => new
                    {
                        BookingEntryId = c.Int(nullable: false, identity: true),
                        ClientId = c.Int(nullable: false),
                        SpotId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookingEntryId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .ForeignKey("dbo.Spots", t => t.SpotId, cascadeDelete: true)
                .Index(t => t.ClientId)
                .Index(t => t.SpotId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientId = c.Int(nullable: false, identity: true),
                        ClientName = c.String(nullable: false, maxLength: 80),
                        BirthDate = c.DateTime(nullable: false),
                        Age = c.Int(nullable: false),
                        Picture = c.String(),
                        MaritalStatus = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ClientId);
            
            CreateTable(
                "dbo.Spots",
                c => new
                    {
                        SpotId = c.Int(nullable: false, identity: true),
                        SpotName = c.String(nullable: false, maxLength: 80),
                    })
                .PrimaryKey(t => t.SpotId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookingEntries", "SpotId", "dbo.Spots");
            DropForeignKey("dbo.BookingEntries", "ClientId", "dbo.Clients");
            DropIndex("dbo.BookingEntries", new[] { "SpotId" });
            DropIndex("dbo.BookingEntries", new[] { "ClientId" });
            DropTable("dbo.Spots");
            DropTable("dbo.Clients");
            DropTable("dbo.BookingEntries");
        }
    }
}
