namespace EducationSalvation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRatingModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CommentModels", "PublicationModelId", "dbo.PublicationModels");
            DropForeignKey("dbo.TagModelPublicationModels", "PublicationModel_Id", "dbo.PublicationModels");
            DropForeignKey("dbo.TagModelPublicationModels", "TagModel_Id", "dbo.TagModels");
            DropForeignKey("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", "dbo.MedalModels");
            DropIndex("dbo.CommentModels", new[] { "PublicationModelId" });
            DropIndex("dbo.TagModelPublicationModels", new[] { "TagModel_Id" });
            DropIndex("dbo.TagModelPublicationModels", new[] { "PublicationModel_Id" });
            DropIndex("dbo.MedalModelAdditionalUserInfoes", new[] { "MedalModel_Id" });
            DropPrimaryKey("dbo.CommentModels");
            DropPrimaryKey("dbo.PublicationModels");
            DropPrimaryKey("dbo.TagModels");
            DropPrimaryKey("dbo.MedalModels");
            DropPrimaryKey("dbo.MedalModelAdditionalUserInfoes");
            DropPrimaryKey("dbo.TagModelPublicationModels");
            CreateTable(
                "dbo.RatingModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.Int(nullable: false),
                        PublicationModelId = c.Int(nullable: false),
                        AdditionalUserInfoId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PublicationModels", t => t.PublicationModelId, cascadeDelete: true)
                .ForeignKey("dbo.AdditionalUserInfoes", t => t.AdditionalUserInfoId)
                .Index(t => t.PublicationModelId)
                .Index(t => t.AdditionalUserInfoId);
            
            AddColumn("dbo.TagModels", "Content", c => c.String());
            AlterColumn("dbo.CommentModels", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.CommentModels", "PublicationModelId", c => c.Int(nullable: false));
            AlterColumn("dbo.PublicationModels", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.TagModels", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.MedalModels", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.TagModelPublicationModels", "TagModel_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.TagModelPublicationModels", "PublicationModel_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.CommentModels", "Id");
            AddPrimaryKey("dbo.PublicationModels", "Id");
            AddPrimaryKey("dbo.TagModels", "Id");
            AddPrimaryKey("dbo.MedalModels", "Id");
            AddPrimaryKey("dbo.MedalModelAdditionalUserInfoes", new[] { "MedalModel_Id", "AdditionalUserInfo_Id" });
            AddPrimaryKey("dbo.TagModelPublicationModels", new[] { "TagModel_Id", "PublicationModel_Id" });
            CreateIndex("dbo.CommentModels", "PublicationModelId");
            CreateIndex("dbo.TagModelPublicationModels", "TagModel_Id");
            CreateIndex("dbo.TagModelPublicationModels", "PublicationModel_Id");
            CreateIndex("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id");
            AddForeignKey("dbo.CommentModels", "PublicationModelId", "dbo.PublicationModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagModelPublicationModels", "PublicationModel_Id", "dbo.PublicationModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagModelPublicationModels", "TagModel_Id", "dbo.TagModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", "dbo.MedalModels", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", "dbo.MedalModels");
            DropForeignKey("dbo.TagModelPublicationModels", "TagModel_Id", "dbo.TagModels");
            DropForeignKey("dbo.TagModelPublicationModels", "PublicationModel_Id", "dbo.PublicationModels");
            DropForeignKey("dbo.CommentModels", "PublicationModelId", "dbo.PublicationModels");
            DropForeignKey("dbo.RatingModels", "AdditionalUserInfoId", "dbo.AdditionalUserInfoes");
            DropForeignKey("dbo.RatingModels", "PublicationModelId", "dbo.PublicationModels");
            DropIndex("dbo.MedalModelAdditionalUserInfoes", new[] { "MedalModel_Id" });
            DropIndex("dbo.TagModelPublicationModels", new[] { "PublicationModel_Id" });
            DropIndex("dbo.TagModelPublicationModels", new[] { "TagModel_Id" });
            DropIndex("dbo.RatingModels", new[] { "AdditionalUserInfoId" });
            DropIndex("dbo.RatingModels", new[] { "PublicationModelId" });
            DropIndex("dbo.CommentModels", new[] { "PublicationModelId" });
            DropPrimaryKey("dbo.TagModelPublicationModels");
            DropPrimaryKey("dbo.MedalModelAdditionalUserInfoes");
            DropPrimaryKey("dbo.MedalModels");
            DropPrimaryKey("dbo.TagModels");
            DropPrimaryKey("dbo.PublicationModels");
            DropPrimaryKey("dbo.CommentModels");
            AlterColumn("dbo.TagModelPublicationModels", "PublicationModel_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TagModelPublicationModels", "TagModel_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.MedalModels", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.TagModels", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.PublicationModels", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.CommentModels", "PublicationModelId", c => c.String(maxLength: 128));
            AlterColumn("dbo.CommentModels", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.TagModels", "Content");
            DropTable("dbo.RatingModels");
            AddPrimaryKey("dbo.TagModelPublicationModels", new[] { "TagModel_Id", "PublicationModel_Id" });
            AddPrimaryKey("dbo.MedalModelAdditionalUserInfoes", new[] { "MedalModel_Id", "AdditionalUserInfo_Id" });
            AddPrimaryKey("dbo.MedalModels", "Id");
            AddPrimaryKey("dbo.TagModels", "Id");
            AddPrimaryKey("dbo.PublicationModels", "Id");
            AddPrimaryKey("dbo.CommentModels", "Id");
            CreateIndex("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id");
            CreateIndex("dbo.TagModelPublicationModels", "PublicationModel_Id");
            CreateIndex("dbo.TagModelPublicationModels", "TagModel_Id");
            CreateIndex("dbo.CommentModels", "PublicationModelId");
            AddForeignKey("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", "dbo.MedalModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagModelPublicationModels", "TagModel_Id", "dbo.TagModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TagModelPublicationModels", "PublicationModel_Id", "dbo.PublicationModels", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CommentModels", "PublicationModelId", "dbo.PublicationModels", "Id");
        }
    }
}
