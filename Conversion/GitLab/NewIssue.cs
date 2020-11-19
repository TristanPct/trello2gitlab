using System;
using System.Collections.Generic;

namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/issues.html#new-issue
    /// </summary>
    public class NewIssue
    {
        public IEnumerable<int> AssisgneeIds { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Description { get; set; }

        public DateTime? DueDate { get; set; }

        public string Labels { get; set; }

        public int? MilestoneId { get; set; }

        public string Title { get; set; }
    }
}
