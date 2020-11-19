using System;
using System.Collections.Generic;

namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/issues.html
    /// </summary>
    public class Issue
    {
        public int Id { get; set; }

        public int Iid { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public IReadOnlyList<User> Assignees { get; set; }

        public IReadOnlyList<string> Labels { get; set; }

        public Milestone Milestone { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime ClosedAt { get; set; }

        public User ClosedBy { get; set; }
    }
}
