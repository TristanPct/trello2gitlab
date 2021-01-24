using System.Collections.Generic;

namespace Trello2GitLab.Conversion.Trello
{
    public class Board
    {
        public string Id { get; set; }

        public IReadOnlyList<Card> Cards { get; set; }

        public IReadOnlyList<Action> Actions { get; set; }

        public IReadOnlyList<Checklist> Checklists { get; set; }

        public IReadOnlyList<List> Lists { get; set; }
    }
}
