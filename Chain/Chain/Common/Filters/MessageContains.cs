using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chain.Common.Filters
{
    public class MessageContains : EventFilter
    {
        private string Match;

        public MessageContains(string match)
        {
            Match = match;
        }

        public override bool OnExecute(Event e)
        {
            if (!(e is ChatMessage))
                return false;

            var chatMessage = (ChatMessage)e;

            return chatMessage.Message.Contains(Match);
        }
    }
}
