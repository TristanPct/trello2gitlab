using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trello2GitLab.Conversion.Trello
{
    public class List
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool Closed { get; set; }

        public Action CloseAction { get; set; }
    }
}
