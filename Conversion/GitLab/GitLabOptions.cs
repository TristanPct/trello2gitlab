namespace Trello2GitLab.Conversion.GitLab
{
    public class GitLabOptions
    {
        /// <summary>
        /// GitLab server URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// GitLab private access token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Tells if the private token has sudo right.
        /// </summary>
        public bool Sudo { get; set; }

        /// <summary>
        /// GitLab target project ID.
        /// </summary>
        public int ProjectId { get; set; }
    }
}
