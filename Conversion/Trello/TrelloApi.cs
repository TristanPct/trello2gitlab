using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Trello2GitLab.Conversion.Trello
{
    /// <summary>
    /// Trello Api helper.
    /// </summary>
    internal class TrelloApi : IDisposable
    {
        protected static readonly JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            },
            DateFormatString = "yyyy-MM-ddTHH:mm:ss.FFFK",
            NullValueHandling = NullValueHandling.Ignore,
        };

        protected readonly HttpClient client;

        /// <summary>
        /// Trello Api helper.
        /// </summary>
        public TrelloApi(TrelloOptions options)
        {
            BaseUrl = $"https://api.trello.com/1/boards/{options.BoardId}";
            Key = options.Key;
            Token = options.Token;
            Include = options.Include;
            client = new HttpClient();
        }

        public string BaseUrl { get; }

        public string Key { get; }

        public string Token { get; }

        public string Include { get; }

        public void Dispose()
        {
            client?.Dispose();
        }

        /// <summary>
        /// Gets all useful data from a Trello board (including actions through multiple requests).
        /// </summary>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<Board> GetBoard()
        {
            var board = await GetBasicBoardData();

            board.Actions = await GetAllActions();

            return board;
        }

        /// <summary>
        /// Makes an asynchronous request to Trello API.
        /// </summary>
        /// <typeparam name="T">Fetched data type.</typeparam>
        /// <param name="endpoint">Target endpoint (starting with `/`).</param>
        /// <param name="parameters">Query parameters.</param>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        protected async Task<T> Request<T>(string endpoint, string parameters)
        {
            var url = $"{BaseUrl}{endpoint}?key={Key}&token={Token}&{parameters}";

            using (var response = await client.GetAsync(url))
            using (var content = response.Content)
            {
                var contentString = await content.ReadAsStringAsync();

                if ((int)response.StatusCode >= 400)
                    throw new ApiException(response, contentString);

                return JsonConvert.DeserializeObject<T>(contentString, jsonSettings);
            }
        }

        /// <summary>
        /// Gets all useful information of a Trello board (except actions).
        /// </summary>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        protected async Task<Board> GetBasicBoardData()
        {
            return await Request<Board>("", $"fields=none&cards={Include}&checklists=all&lists=all");
        }

        /// <summary>
        /// Gets all actions of a Trello board.
        /// This endpoint has a limit of 1000 actions. To get all actions the method needs to make multiple calls, giving the ID of the last action.
        /// </summary>
        /// <exception cref="ApiException"></exception>
        /// <exception cref="HttpRequestException"></exception>
        protected async Task<IReadOnlyList<Action>> GetAllActions()
        {
            const int limit = 1000;
            var actions = new List<Action>();

            IReadOnlyList<Action> apiResponseActions;
            do
            {
                apiResponseActions = await Request<IReadOnlyList<Action>>("/actions", $"limit={limit}&filter=createCard,updateCard,commentCard,updateList&before={actions.LastOrDefault()?.Id}");

                actions.AddRange(apiResponseActions);
            } while (apiResponseActions.Count == limit);

            return actions;
        }
    }
}
