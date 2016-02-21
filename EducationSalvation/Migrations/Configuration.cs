namespace EducationSalvation.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EducationSalvation.Models.PublicationModelContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EducationSalvation.Models.PublicationModelContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //context.AdditionalUserInfoes.Add(new Models.AdditionalUserInfo()
            //{
            //    Id = "44d5f6dd-415d-49b5-a55d-5ff9c7fce991", Age = 12, FirstName = "", Gender = "", Interests = "", LastName = "", Location = ""
            //});
            //context.CommentModels.Add(new Models.CommentModel()
            //{
            //    AdditionalUserInfoId = "44d5f6dd-415d-49b5-a55d-5ff9c7fce991",
            //    Content = "FUCK ME",
            //    Date = "0.0.0 12:47:12",
            //    Id = "0",
            //    Rating = 0,
            //    PublicationModelId = "0"
            //});
            //var model = context.AdditionalUserInfoes.FirstOrDefault(u => u.FirstName == "Velior");
            //model.Nickname = "Velior";
            //context.NicknameModels.Add(new Models.NicknameModel() { Nickname = "Velior", UserId = model.Id});
            //context.SaveChanges();

            //model = context.AdditionalUserInfoes.FirstOrDefault(u => u.FirstName == "Vasya");
            //model.Nickname = "nagibator";
            //context.NicknameModels.Add(new Models.NicknameModel() { Nickname = "nagibator", UserId = model.Id });
            //context.MedalModels.Add(new Models.MedalModel()
            //{
            //    Url = "~/StaticResources/Medals/first-post.jpg",
            //    Description = "Medal for first post",
            //});
            //context.MedalModels.Add(new Models.MedalModel()
            //{
            //    Url = "~/StaticResources/Medals/ten-posts.jpg",
            //    Description = "Medal for tenth post",
            //});
            //context.MedalModels.Add(new Models.MedalModel()
            //{
            //    Url = "~/StaticResources/Medals/ten-comments.jpg",
            //    Description = "Medal for ten comments",
            //});
            //context.MedalModels.Add(new Models.MedalModel()
            //{
            //    Url = "~/StaticResources/Medals/ten-likes.jpg",
            //    Description = "Medal for ten likes on your comments!",
            //});

        }
    }
}
