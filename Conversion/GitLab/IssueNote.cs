using System;

namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/notes.html#issues
    /// </summary>
    public class IssueNote
    {
        public int Id { get; set; }

        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User Author { get; set; }
    }
}
