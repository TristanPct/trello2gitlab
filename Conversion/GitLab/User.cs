
namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    ///https://docs.gitlab.com/ee/api/users.html
    /// </summary>
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public bool IsAdmin { get; set; }
    }
}
