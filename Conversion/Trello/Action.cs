using System;

namespace Trello2GitLab.Conversion.Trello
{
    public class Action
    {
        public string Id { get; set; }

        public string IdMemberCreator { get; set; }

        public ActionData Data { get; set; }

        public string Type { get; set; }

        public DateTime Date { get; set; }
    }
}
