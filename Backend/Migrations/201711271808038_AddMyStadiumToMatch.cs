namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMyStadiumToMatch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stadia",
                c => new
                    {
                        StadiumId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.StadiumId)
                .Index(t => t.Name, unique: true, name: "Stadium_Name_Index");
            
            
            AddColumn("dbo.Matches", "StadiumId", c => c.Int());

            // ADD THIS BY HAND

            Sql(@"INSERT INTO dbo.Stadia(Name) VALUES('Estadio Bellavista')");
                         
            Sql(@"UPDATE dbo.Matches SET StadiumId = 1
              where StadiumId IS NULL");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Stadia", "Stadium_Name_Index");
            DropColumn("dbo.Matches", "StadiumId");
            DropTable("dbo.Stadia");
        }
    }
}
