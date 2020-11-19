using Trello2GitLab.Conversion.GitLab;
using Trello2GitLab.Conversion.Trello;

namespace Trello2GitLab.Conversion
{
    public class ConverterOptions
    {
        /// <summary>
        /// Trello specific options.
        /// </summary>
        public TrelloOptions Trello { get; set; }

        /// <summary>
        /// GitLa specific options.
        /// </summary>
        public GitLabOptions GitLab { get; set; }

        /// <summary>
        /// Card to issue associations.
        /// </summary>
        public AssociationsOptions Associations { get; set; }
    }
}
