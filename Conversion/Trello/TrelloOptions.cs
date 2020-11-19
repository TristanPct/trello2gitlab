namespace Trello2GitLab.Conversion.Trello
{
    public class TrelloOptions
    {
        /// <summary>
        /// Trello API Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Trello API Token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Trello board ID.
        /// </summary>
        public string BoardId { get; set; }
    }
}
