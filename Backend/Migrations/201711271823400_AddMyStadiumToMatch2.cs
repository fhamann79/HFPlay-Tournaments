namespace Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMyStadiumToMatch2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Matches", "StadiumId", c => c.Int(nullable: false));
            CreateIndex("dbo.Matches", "StadiumId");
            AddForeignKey("dbo.Matches", "StadiumId", "dbo.Stadia", "StadiumId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Matches", "StadiumId", "dbo.Stadia");
            DropIndex("dbo.Matches", new[] { "StadiumId" });
            AlterColumn("dbo.Matches", "StadiumId", c => c.Int());
        }
    }
}
