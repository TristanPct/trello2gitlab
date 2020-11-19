using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

#if DEBUG
namespace Trello2GitLab.Conversion.GitLab
{
    internal static class GitLabApiDebugExtensions
    {
        /// <summary>
        /// Deletes all project issues.
        /// </summary>
        /// <remarks>
        /// Used for internal tests.
        /// </remarks>
        internal static async Task DeleteAllIssues(this GitLabApi api)
        {
            var issues = await api.GetAllIssues();

            foreach (var issue in issues)
            {
                await api.Request(HttpMethod.Delete, $"/issues/{issue.Iid}");
            }
        }

        /// <summary>
        /// Gets all issues of a GitLab project.
        /// This endpoint has a limit of 100 issues. To get all issues the method needs to make multiple calls, giving the page.
        /// </summary>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        internal static async Task<IReadOnlyList<Issue>> GetAllIssues(this GitLabApi api)
        {
            const int limit = 100;
            var issues = new List<Issue>();

            int page = 1;

            IReadOnlyList<Issue> apiResponseIssues;
            do
            {
                apiResponseIssues = await api.Request<IReadOnlyList<Issue>>(HttpMethod.Get, $"/issues?scope=all&per_page={limit}&page={page++}");

                issues.AddRange(apiResponseIssues);
            } while (apiResponseIssues.Count == limit);

            return issues;
        }
    }
}
#endif
