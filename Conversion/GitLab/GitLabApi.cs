using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Trello2GitLab.Conversion.GitLab
{
    /// <summary>
    /// GitLab Api helper.
    /// </summary>
    internal class GitLabApi : IDisposable
    {
        protected static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            },
            DateFormatString = "yyyy-MM-ddTHH:mm:ssK",
            NullValueHandling = NullValueHandling.Ignore,
        };

        protected readonly HttpClient client;

        /// <summary>
        /// GitLab Api helper.
        /// </summary>
        public GitLabApi(GitLabOptions options)
        {
            BaseUrl = $"{options.Url}/api/v4";
            ProjectUrl = $"{BaseUrl}/projects/{options.ProjectId}";
            Token = options.Token;
            Sudo = options.Sudo;

            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", Token);
        }

        public string BaseUrl { get; }

        public string ProjectUrl { get; }

        public string Token { get; }

        public bool Sudo { get; }

        public void Dispose()
        {
            client?.Dispose();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        public async Task<IReadOnlyList<User>> GetAllUsers()
        {
            return await Request<IReadOnlyList<User>>(HttpMethod.Get, "/users", projectBasedUrl: false);
        }

        /// <summary>
        /// Edits an user.
        /// </summary>
        /// <param name="user">The user to edit</param>
        public async Task<User> EditUser(EditUser user)
        {
            return await Request<User>(HttpMethod.Put, $"/users/{user.Id}", null, user, projectBasedUrl: false);
        }

        /// <summary>
        /// Creates an issue in the target GitLab server.
        /// </summary>
        /// <param name="newIssue">The issue object to create.</param>
        /// <param name="createdBy">ID of the user creating the issue.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Issue> CreateIssue(NewIssue newIssue, int? createdBy = null)
        {
            return await Request<Issue>(HttpMethod.Post, "/issues", createdBy, newIssue);
        }

        /// <summary>
        /// Edits an issue.
        /// </summary>
        /// <param name="editIssue">The issue object to edit.</param>
        /// <param name="editedBy">ID of the user editing the issue.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Issue> EditIssue(EditIssue editIssue, int? editedBy = null)
        {
            return await Request<Issue>(HttpMethod.Put, $"/issues/{editIssue.IssueIid}", editedBy, editIssue);
        }

        /// <summary>
        /// Adds comment to an issue.
        /// </summary>
        /// <param name="issue">The issue object to comment.</param>
        /// <param name="comment">The comment object to add.</param>
        /// <param name="commentedBy">ID of the user commenting the issue.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IssueNote> CommentIssue(Issue issue, NewIssueNote comment, int? commentedBy = null)
        {
            return await Request<IssueNote>(HttpMethod.Post, $"/issues/{issue.Iid}/notes", commentedBy, comment);
        }

        /// <summary>
        /// Builds a GitLab Api URL.
        /// </summary>
        /// <param name="endpoint">Target endpoint (starting with `/`).</param>
        /// <param name="projectBasedUrl">Tells if the API URL targets the project.</param>
        protected string Url(string endpoint, bool projectBasedUrl)
        {
            return (projectBasedUrl ? ProjectUrl : BaseUrl) + endpoint;
        }

        /// <summary>
        /// Makes an asynchronous request to GitLab API.
        /// </summary>
        /// <typeparam name="T">Fetched data type.</typeparam>
        /// <param name="method">The HTTP method.</param>
        /// <param name="endpoint">Target endpoint (starting with `/`).</param>
        /// <param name="userId">User to impersonate (if sudo).</param>
        /// <param name="serializableContent">Serializable content to send.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        internal async Task<T> Request<T>(HttpMethod method, string endpoint, int? userId, object serializableContent, bool projectBasedUrl = true)
        {
            var serializedContent = JsonConvert.SerializeObject(serializableContent, jsonSettings);

            using (var content = new StringContent(serializedContent, Encoding.UTF8, "application/json"))
            {
                return await Request<T>(method, endpoint, userId, content, projectBasedUrl);
            }
        }

        /// <summary>
        /// Makes an asynchronous request to GitLab API (without body).
        /// </summary>
        /// <typeparam name="T">Fetched data type.</typeparam>
        /// <param name="method">The HTTP method.</param>
        /// <param name="endpoint">Target endpoint (starting with `/`).</param>
        /// <param name="userId">User to impersonate (if sudo).</param>
        /// <param name="content">HTTP request content.</param>
        /// <param name="projectBasedUrl">Tells if the API URL targets the project.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        internal async Task<T> Request<T>(HttpMethod method, string endpoint, int? userId = null, HttpContent content = null, bool projectBasedUrl = true)
        {
            using (var request = new HttpRequestMessage(method, Url(endpoint, projectBasedUrl)))
            {
                if (Sudo && userId != null)
                {
                    request.Headers.Add("Sudo", userId.ToString());
                }

                if (content != null)
                {
                    request.Content = content;
                }

                using (var response = await client.SendAsync(request))
                using (var responseContent = response.Content)
                {
                    var contentString = await responseContent.ReadAsStringAsync();

                    if ((int)response.StatusCode >= 400)
                        throw new ApiException(response, contentString);

                    return JsonConvert.DeserializeObject<T>(contentString, jsonSettings);
                }
            }
        }

        /// <summary>
        /// Makes an asynchronous request to GitLab API (without body nor response).
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="endpoint">Target endpoint (starting with `/`).</param>
        /// <param name="projectBasedUrl">Tells if the API URL targets the project.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        internal async Task Request(HttpMethod method, string endpoint, bool projectBasedUrl = true)
        {
            using (var request = new HttpRequestMessage(method, Url(endpoint, projectBasedUrl)))
            using (var response = await client.SendAsync(request))
            using (var responseContent = response.Content)
            {
                var contentString = await responseContent.ReadAsStringAsync();

                if ((int)response.StatusCode >= 400)
                    throw new ApiException(response, contentString);
            }
        }
    }
}
