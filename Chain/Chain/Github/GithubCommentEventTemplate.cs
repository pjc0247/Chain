using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Github
{
    public class GithubCommentEventTemplate : IMessageTemplate
    {
        public GithubCommentEventTemplate()
        {
        }

        public string OnExecute(TaskContext Context)
        {
            var ev = Context.Get<CommentEvent>();

            return $"[New Comment] {ev.From.Owner}/{ev.From.RepositoryName} : {ev.User.Name}/ {ev.Message}";
        }
    }
}
