using System.Collections.Generic;

namespace Trello2GitLab.Conversion
{
    public class AssociationsOptions
    {
        /// <summary>
        /// Trello label ID to GitLab label name.
        /// </summary>
        public Dictionary<string, string> Labels_Labels { get; set; }

        /// <summary>
        /// Trello list ID to GitLab label name.
        /// </summary>
        public Dictionary<string, string> Lists_Labels { get; set; }

        /// <summary>
        /// Trello label ID to GitLab milestone ID.
        /// </summary>
        public Dictionary<string, int> Labels_Milestones { get; set; }

        /// <summary>
        /// Trello list ID to GitLab milestone ID.
        /// </summary>
        public Dictionary<string, int> Lists_Milestones { get; set; }

        /// <summary>
        /// Trello member ID to GitLab user ID.
        /// </summary>
        public Dictionary<string, int> Members_Users { get; set; }
    }
}
