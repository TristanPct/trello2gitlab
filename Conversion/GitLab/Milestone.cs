namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// https://docs.gitlab.com/ee/api/milestones.html
    /// </summary>
    public class Milestone
    {
        public int Id { get; set; }

        public int Iid { get; set; }
    }
}
