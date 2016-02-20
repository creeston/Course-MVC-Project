using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace EducationSalvation.Models
{
    public class PublicationCreatingModel
    {
        public string Template { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public string Images { get; set; }
        public string Videos { get; set; }
        public string Markdown { get; set; }
        public string Date { get; set; }
    }
    public class PublicationShowingModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string[] Images { get; set; }
        public string[] Videos { get; set; }
        public CommentShowingModel[] Comments { get; set; }
        public string Markdown { get; set; }
        public string Date { get; set; }
        public int Stars { get; set; }
        public bool IsUserAlreadyGraduateIt { get; set; }
    }
    public class PublicationThumbnailModel
    {
        public int Id { get; set; }
        public string UserNickname { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        public string Date { get; set; }
        public int Stars { get; set; }
    }

    public class PublicationModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Template { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
       
        public string Images { get; set; }
        public string Videos { get; set; }
        public string Markdown { get; set; }
        public string Date { get; set; }
        public int Stars { get; set; }

        public PublicationModel()
        {
            CommentModels = new List<CommentModel>();
            TagModels = new HashSet<TagModel>();
            RatingModels = new List<RatingModel>();
        }
        public virtual ICollection<CommentModel> CommentModels { get; set; }
        public virtual ICollection<TagModel> TagModels { get; set; }
        public virtual ICollection<RatingModel> RatingModels { get; set; }

        public string AdditionalUserInfoId { get; set; }
        [ForeignKey("AdditionalUserInfoId")]
        public virtual AdditionalUserInfo User { get; set; }
    }

    public class TagModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public int Rate { get; set; }

        public virtual ICollection<PublicationModel> PublicationModels { get; set; }
    }

    public class CommentSendingModel
    {
        public int PublicationId { get; set; }
        public string Content { get; set; }
        public string Date { get; set; }
    }
    public class CommentShowingModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public int Rating { get; set; }
        public bool IsUserAlreadyLikedIt { get; set; }
    }

    public class CommentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Content { get; set; }

        public int PublicationModelId { get; set; }
        [ForeignKey("PublicationModelId")]
        public virtual PublicationModel Publication { get; set; }

        public string AdditionalUserInfoId { get; set; }
        [ForeignKey("AdditionalUserInfoId")]
        public virtual AdditionalUserInfo User { get; set; }

        public string Date { get; set; }
        public int Rating { get; set; }

        public CommentModel()
        {
            LikeModels = new List<LikeModel>();
        }

        public virtual ICollection<LikeModel> LikeModels { get; set; }
    }

    public class Template
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public class NicknameModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nickname { get; set; }
        public string UserId { get; set; }
    }

    public class ShowingAdditionalUserInfo
    {
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Interests { get; set; }

        public PublicationThumbnailModel[] Publications;
        
    }

    public class EditableAdditionalUserInfo
    {
        public string Nickname { get; set; }

        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Interests { get; set; }

        public string[] Nicknames { get; set; }
    }

    //DELETE
    public class CreatableAdditionalUserInfo
    {
        public string Nickname { get; set; }

        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Interests { get; set; }
        public string[] Nicknames { get; set; }
    }

    public class AdditionalUserInfo
    {
        [Key]
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string Interests { get; set; }

        public AdditionalUserInfo()
        {
            CommentModels = new List<CommentModel>();
            PublicationModels = new List<PublicationModel>();
            RatingModels = new List<RatingModel>();
            LikeModels = new List<LikeModel>();
        }

        public virtual ICollection<PublicationModel> PublicationModels { get; set; }
        public virtual ICollection<CommentModel> CommentModels { get; set; }
        public virtual ICollection<MedalModel> MedalModels { get; set; }
        public virtual ICollection<RatingModel> RatingModels { get; set; }
        public virtual ICollection<LikeModel> LikeModels { get; set; }
    }

    public class MedalModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public MedalModel()
        {
            AdditionalUserInfoes = new HashSet<AdditionalUserInfo>();
        }

        public virtual ICollection<AdditionalUserInfo> AdditionalUserInfoes { get; set; }
    }

    public class RatingSendingModel
    {
        public int PublicationId { get; set; }
        public int Value { get; set; }
    }

    public class RatingModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }

        public int PublicationModelId { get; set; }
        [ForeignKey("PublicationModelId")]
        public virtual PublicationModel Publication { get; set; }

        public string AdditionalUserInfoId { get; set; }
        [ForeignKey("AdditionalUserInfoId")]
        public virtual AdditionalUserInfo User { get; set; }
    }

    public class LikeModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public int CommentModelId { get; set; }
        [ForeignKey("CommentModelId")]
        public virtual CommentModel Comment { get; set; }

        public string AdditionalUserInfoId { get; set; }
        [ForeignKey("AdditionalUserInfoId")]
        public virtual AdditionalUserInfo User { get; set; }
    }

    public class PublicationModelContext : DbContext
    {
        public PublicationModelContext() : base("DefaultConnection") { }
        public DbSet<PublicationModel> PublicationModels { get; set; }
        public DbSet<TagModel> TagModels { get; set; }
        public DbSet<AdditionalUserInfo> AdditionalUserInfoes { get; set; }
        public DbSet<MedalModel> MedalModels { get; set; }
        public DbSet<CommentModel> CommentModels { get; set; }
        public DbSet<RatingModel> RatingModels { get; set; }
        public DbSet<LikeModel> LikeModels { get; set; }
        public DbSet<NicknameModel> NicknameModels { get; set; }
    }

}