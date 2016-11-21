using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain
{
    public class TextMessageEvent : Event
    {
        public string Message;
    }

    public class LocalCopy : Event
    {
        public string Name;
        public string Path;
    }

    public class BuildResult : Event
    {
        public LocalCopy Copy;
        public bool Success;
    }

    public class ChatMessage : Event
    {
        public string Room;
        public string Sender;
        public string Message;
    }
    public class ChatImageMessage : ChatMessage
    {
        public string ImageUrl;
    }

    public class PendingRequest : Event
    {
    }
}
