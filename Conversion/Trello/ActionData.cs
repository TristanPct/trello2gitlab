using System.Collections.Generic;

namespace Trello2GitLab.Conversion.Trello
{
    public class ActionData
    {
        public string Text { get; set; }

        public IReadOnlyDictionary<string, object> Old { get; set; }

        public Card Card { get; set; }

        public List List { get; set; }
    }
}
