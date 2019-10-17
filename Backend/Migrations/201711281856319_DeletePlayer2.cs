namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeletePlayer2 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Players");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        FirtsName = c.String(nullable: false, maxLength: 75),
                        LastName = c.String(nullable: false, maxLength: 75),
                        IdentificationCard = c.String(nullable: false, maxLength: 10),
                        Email = c.String(maxLength: 75),
                        Birthdate = c.DateTime(nullable: false),
                        Photo = c.String(),
                    })
                .PrimaryKey(t => t.PlayerId);
            
        }
    }
}
