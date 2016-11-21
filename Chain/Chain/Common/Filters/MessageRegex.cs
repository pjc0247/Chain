using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chain.Common.Filters
{
    public class MessageRegex : EventFilter
    {
        private Regex Regex;

        public MessageRegex(Regex regex)
        {
            Regex = regex;
        }

        public override bool OnExecute(Event e)
        {
            if (!(e is ChatMessage))
                return false;

            var chatMessage = (ChatMessage)e;

            return Regex.IsMatch(chatMessage.Message);
        }
    }
}
