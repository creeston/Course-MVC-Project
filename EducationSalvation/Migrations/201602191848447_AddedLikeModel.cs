namespace EducationSalvation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLikeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LikeModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentModelId = c.Int(nullable: false),
                        AdditionalUserInfoId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommentModels", t => t.CommentModelId, cascadeDelete: true)
                .ForeignKey("dbo.AdditionalUserInfoes", t => t.AdditionalUserInfoId)
                .Index(t => t.CommentModelId)
                .Index(t => t.AdditionalUserInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LikeModels", "AdditionalUserInfoId", "dbo.AdditionalUserInfoes");
            DropForeignKey("dbo.LikeModels", "CommentModelId", "dbo.CommentModels");
            DropIndex("dbo.LikeModels", new[] { "AdditionalUserInfoId" });
            DropIndex("dbo.LikeModels", new[] { "CommentModelId" });
            DropTable("dbo.LikeModels");
        }
    }
}
