namespace EducationSalvation.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalUserInfoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Gender = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Age = c.Int(nullable: false),
                        Location = c.String(),
                        Interests = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommentModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Content = c.String(),
                        PublicationModelId = c.String(maxLength: 128),
                        AdditionalUserInfoId = c.String(maxLength: 128),
                        Date = c.String(),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PublicationModels", t => t.PublicationModelId)
                .ForeignKey("dbo.AdditionalUserInfoes", t => t.AdditionalUserInfoId)
                .Index(t => t.PublicationModelId)
                .Index(t => t.AdditionalUserInfoId);
            
            CreateTable(
                "dbo.PublicationModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Template = c.String(),
                        Title = c.String(),
                        Description = c.String(),
                        Images = c.String(),
                        Videos = c.String(),
                        Markdown = c.String(),
                        Date = c.String(),
                        Stars = c.Int(nullable: false),
                        AdditionalUserInfoId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AdditionalUserInfoes", t => t.AdditionalUserInfoId)
                .Index(t => t.AdditionalUserInfoId);
            
            CreateTable(
                "dbo.TagModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Rate = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MedalModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Url = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TagModelPublicationModels",
                c => new
                    {
                        TagModel_Id = c.String(nullable: false, maxLength: 128),
                        PublicationModel_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.TagModel_Id, t.PublicationModel_Id })
                .ForeignKey("dbo.TagModels", t => t.TagModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.PublicationModels", t => t.PublicationModel_Id, cascadeDelete: true)
                .Index(t => t.TagModel_Id)
                .Index(t => t.PublicationModel_Id);
            
            CreateTable(
                "dbo.MedalModelAdditionalUserInfoes",
                c => new
                    {
                        MedalModel_Id = c.String(nullable: false, maxLength: 128),
                        AdditionalUserInfo_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.MedalModel_Id, t.AdditionalUserInfo_Id })
                .ForeignKey("dbo.MedalModels", t => t.MedalModel_Id, cascadeDelete: true)
                .ForeignKey("dbo.AdditionalUserInfoes", t => t.AdditionalUserInfo_Id, cascadeDelete: true)
                .Index(t => t.MedalModel_Id)
                .Index(t => t.AdditionalUserInfo_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MedalModelAdditionalUserInfoes", "AdditionalUserInfo_Id", "dbo.AdditionalUserInfoes");
            DropForeignKey("dbo.MedalModelAdditionalUserInfoes", "MedalModel_Id", "dbo.MedalModels");
            DropForeignKey("dbo.CommentModels", "AdditionalUserInfoId", "dbo.AdditionalUserInfoes");
            DropForeignKey("dbo.PublicationModels", "AdditionalUserInfoId", "dbo.AdditionalUserInfoes");
            DropForeignKey("dbo.TagModelPublicationModels", "PublicationModel_Id", "dbo.PublicationModels");
            DropForeignKey("dbo.TagModelPublicationModels", "TagModel_Id", "dbo.TagModels");
            DropForeignKey("dbo.CommentModels", "PublicationModelId", "dbo.PublicationModels");
            DropIndex("dbo.MedalModelAdditionalUserInfoes", new[] { "AdditionalUserInfo_Id" });
            DropIndex("dbo.MedalModelAdditionalUserInfoes", new[] { "MedalModel_Id" });
            DropIndex("dbo.TagModelPublicationModels", new[] { "PublicationModel_Id" });
            DropIndex("dbo.TagModelPublicationModels", new[] { "TagModel_Id" });
            DropIndex("dbo.PublicationModels", new[] { "AdditionalUserInfoId" });
            DropIndex("dbo.CommentModels", new[] { "AdditionalUserInfoId" });
            DropIndex("dbo.CommentModels", new[] { "PublicationModelId" });
            DropTable("dbo.MedalModelAdditionalUserInfoes");
            DropTable("dbo.TagModelPublicationModels");
            DropTable("dbo.MedalModels");
            DropTable("dbo.TagModels");
            DropTable("dbo.PublicationModels");
            DropTable("dbo.CommentModels");
            DropTable("dbo.AdditionalUserInfoes");
        }
    }
}
