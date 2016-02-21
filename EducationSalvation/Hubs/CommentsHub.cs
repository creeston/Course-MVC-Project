using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using EducationSalvation.Models;

namespace EducationSalvation.Hubs
{
    public class CommentsHub : Hub
    {
        // Отправка сообщений
        public void SendComment()
        {
            Clients.All.updateCommentSection();
        }

    }


}