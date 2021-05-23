using System.Collections.Generic;

namespace Trello2GitLab.Conversion
{
    public enum ConversionStep
    {
        Init,

        FetchingTrelloBoard,
        TrelloBoardFetched,

        GrantAdminPrivileges,
        AdminPrivilegesGranted,

        FetchMilestones,
        MilestonesFetched,

        ConvertingCards,
        CardsConverted,

        RevokeAdminPrivileges,
        AdminPrivilegesRevoked,

        Finished,
    }

    public class ConversionProgressReport
    {
        public ConversionStep CurrentStep { get; }

        public int CurrentIndex { get; }

        public int? TotalElements { get; }

        public IEnumerable<string> Errors { get; }

        internal ConversionProgressReport(ConversionStep step, int current = 0, int? total = null, IEnumerable<string> errors = null)
        {
            CurrentStep = step;
            CurrentIndex = current;
            TotalElements = total;
            Errors = errors;
        }
    }
}
