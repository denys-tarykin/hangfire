namespace Hangfire.Dao.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.users",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        username = c.String(),
                        email = c.String(),
                        password = c.String(),
                        entity_status = c.Int(nullable: false),
                        row_version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.assets",
                c => new
                    {
                        id = c.Long(nullable: false, identity: true),
                        name = c.String(),
                        path = c.String(),
                        entity_status = c.Int(nullable: false),
                        row_version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.assets");
            DropTable("dbo.users");
        }
    }
}
