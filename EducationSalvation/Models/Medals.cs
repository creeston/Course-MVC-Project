using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EducationSalvation.Models
{
    public class FirstPostMedal : IMedalChecker
    {
        public bool CheckConditions(AdditionalUserSummary userSummary)
        {
            if (userSummary.PublicationCount >= 1) return true;
            else return false;
        }
    }

    public class TenPostsMedal : IMedalChecker
    {
        public bool CheckConditions(AdditionalUserSummary userSummary)
        {
            if (userSummary.PublicationCount >= 10) return true;
            else return false;
        }
    }

    public class TenCommentsMedal : IMedalChecker
    {
        public bool CheckConditions(AdditionalUserSummary userSummary)
        {
            if (userSummary.CommentsCount >= 10) return true;
            else return false;
        }
    }

    public class TenLikesMedal : IMedalChecker
    {
        public bool CheckConditions(AdditionalUserSummary userSummary)
        {
            if (userSummary.CommentLikesCount >= 10) return true;
            else return false;
        }
    }
}