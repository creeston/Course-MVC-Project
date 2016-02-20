namespace EducationSalvation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNickname : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NicknameModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nickname = c.String(),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AdditionalUserInfoes", "Nickname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdditionalUserInfoes", "Nickname");
            DropTable("dbo.NicknameModels");
        }
    }
}
