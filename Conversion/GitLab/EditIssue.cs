using System;
using System.Collections.Generic;

namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/issues.html#edit-issue
    /// </summary>
    public class EditIssue
    {
        public int IssueIid { get; set; }

        public IEnumerable<int> AssigneeIds { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public string Labels { get; set; }

        public int? MilestoneId { get; set; }

        public string StateEvent { get; set; }

        public string Title { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
