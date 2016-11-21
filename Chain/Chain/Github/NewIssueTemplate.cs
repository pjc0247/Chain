using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chain.Github
{
    public class NewIssueTemplate : IMessageTemplate
    {
        public NewIssueTemplate()
        {
        }

        public string OnExecute(TaskContext Context)
        {
            var ev = Context.Get<IssueOrPullRequest>();

            return $"[New Issue] {ev.From.Owner}/{ev.From.RepositoryName} : {ev.User.Name}/ {ev.Title}\r\nLink : {ev.Url}";
        }
    }
}
