using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Github
{
    public class UserMentioned : EventFilter
    {
        private string UserName;

        public UserMentioned(string userName)
        {
            UserName = userName;
        }

        public override bool OnExecute(Event e)
        {
            if (!(e is CommentEvent))
                return false;

            var commentEvent = (CommentEvent)e;

            return commentEvent.Message.Contains($"@{UserName}");
        }
    }
}
