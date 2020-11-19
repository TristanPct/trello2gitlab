using System;

namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/notes.html#create-new-issue-note
    /// </summary>
    public class NewIssueNote
    {
        public string Body { get; set; }

        public DateTime? CreatedAt { get; set; }
    }
}
