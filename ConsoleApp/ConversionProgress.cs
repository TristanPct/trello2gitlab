using System;
using Trello2GitLab.Conversion;

namespace Trello2GitLab.ConsoleApp
{
    internal class ConversionProgress : IProgress<ConversionProgressReport>
    {
        private static readonly object messageLock = new object();

        public void Report(ConversionProgressReport value)
        {
            switch (value.CurrentStep)
            {
                case ConversionStep.Init:
                    return;

                case ConversionStep.FetchingTrelloBoard:
                    Print("Fetching Trello board...\n");
                    return;

                case ConversionStep.TrelloBoardFetched:
                    PrintSuccess("Trello board fetched.\n");
                    return;

                case ConversionStep.GrantAdminPrivileges when value.TotalElements == null:
                    Print("Granting admin privileges...\n");
                    return;

                case ConversionStep.GrantAdminPrivileges when value.TotalElements != null && value.Errors == null:
                    Print($"Granting privilege (user {value.CurrentIndex + 1} of {value.TotalElements})\r");
                    return;

                case ConversionStep.AdminPrivilegesGranted:
                    PrintSuccess("\nAdmin privileges granted.\n");
                    return;

                case ConversionStep.FetchMilestones when value.TotalElements == null:
                    Print("Fetching project milestones...\n");
                    return;

                case ConversionStep.FetchMilestones when value.TotalElements != null && value.Errors == null:
                    Print($"Fetching milestone ({value.CurrentIndex + 1} of {value.TotalElements})\r");
                    return;

                case ConversionStep.MilestonesFetched:
                    PrintSuccess("\nProject milestones fetched.\n");
                    return;

                case ConversionStep.ConvertingCards when value.TotalElements == null:
                    Print("Starting cards conversion...\n");
                    return;

                case ConversionStep.ConvertingCards when value.TotalElements != null && value.Errors == null:
                    Print($"Converting card ({value.CurrentIndex + 1} of {value.TotalElements})\r");
                    return;

                case ConversionStep.CardsConverted:
                    PrintSuccess("\nConversion done.\n");
                    return;

                case ConversionStep.RevokeAdminPrivileges when value.TotalElements == null:
                    Print("Revoking admin privileges...\n");
                    return;

                case ConversionStep.RevokeAdminPrivileges when value.TotalElements != null && value.Errors == null:
                    Print($"Revoking privilege (user {value.CurrentIndex + 1} of {value.TotalElements})\r");
                    return;

                case ConversionStep.AdminPrivilegesRevoked:
                    PrintSuccess("\nAdmin privileges revoked.\n");
                    return;

                case ConversionStep.Finished:
                    PrintSuccess("\nConversion done.");
                    return;

                case ConversionStep.GrantAdminPrivileges when value.TotalElements != null && value.Errors != null:
                case ConversionStep.ConvertingCards when value.TotalElements != null && value.Errors != null:
                case ConversionStep.RevokeAdminPrivileges when value.TotalElements != null && value.Errors != null:
                    Print("\n");
                    foreach (var error in value.Errors)
                    {
                        PrintError(error + "\n");
                    }
                    return;
            }
        }

        public void Print(string message)
        {
            lock (messageLock)
            {
                Console.Out.Write(message);
            }
        }

        public void PrintSuccess(string message)
        {
            lock (messageLock)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Out.Write(message);
                Console.ResetColor();
            }
        }

        public void PrintError(string message)
        {
            lock (messageLock)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.Write(message);
                Console.ResetColor();
            }
        }
    }
}
