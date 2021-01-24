using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using Trello2GitLab.Conversion;

namespace Trello2GitLab.ConsoleApp
{
    internal enum ExitCode
    {
        InvalidArguments = -1,
        Success = 0,
        OptionsError = 1,
        ConversionError = 2,
    }

    public static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "-h" || args[0] == "--help")
            {
                ShowHelp();
                return (int)ExitCode.Success;
            }
            else if (args.Length == 1)
            {
                return (int)await RunConversion(args[0]);
            }
#if DEBUG
            else if(args.Length == 2 && (args[1] == "--delete"))
            {
                return (int)await RunDeletion(args[0]);
            }
#endif
            else
            {
                Console.Error.Write("Invalid arguments supplied.\nUse -h option to see help.");
                return (int)ExitCode.InvalidArguments;
            }
        }

        private static void ShowHelp()
        {
            Console.Write(@"
trello2gitlab
Convert Trello cards to GitLab issues.

Usage:
  trello2gitlab path/to/options.json
  trello2gitlab [-h|--help]

Options:
  -h|--help    Show this screen.

Options file format:
  {
      ""trello"": {
          ""key"": <Trello API key (string)>,
          ""token"": <Trello API token (string)>,
          ""boardId"": <Trello board ID (string)>,
          ""include"": <Specifies which cards to include (""all""|""open""|""visible""|""closed"") [default: ""all""]>
      },
      ""gitlab"": {
          ""url"": <GitLab server base URL (string) [default: ""https://gitlab.com""]>,
          ""token"": <GitLab private access token (string)>,
          ""sudo"": <Tells if the private token has sudo rights (bool)>,
          ""projectId"": <GitLab target project ID (int)>
      },
      ""associations"": {
          ""labels_labels"": {
              <Trello label ID (string)>: <GitLab label name (string)>
          },
          ""lists_labels"": {
              <Trello list ID (string)>: <GitLab label name (string)>
          },
          ""labels_milestones"": {
              <Trello label ID (string)>: <GitLab milestone ID (int)>
          },
          ""lists_milestones"": {
              <Trello list ID (string)>: <GitLab milestone ID (int)>
          },
          ""members_users"": {
              <Trello member ID (string)>: <GitLab user ID (int)>
          }
      }
  }
            ".Trim());
        }

        private static async Task<ExitCode> RunConversion(string optionsFilePath)
        {
            if (!TryGetConverterOptions(optionsFilePath, out ConverterOptions options))
            {
                Console.Error.WriteLine($"The options file cannot be located at: {optionsFilePath}");
                return ExitCode.OptionsError;
            }

            using (var converter = new Converter(options))
            {
                bool success = await converter.ConvertAll(new ConversionProgress());

                return success ? ExitCode.Success : ExitCode.ConversionError;
            }
        }

#if DEBUG
        private static async Task<ExitCode> RunDeletion(string optionsFilePath)
        {
            if (!TryGetConverterOptions(optionsFilePath, out ConverterOptions options))
            {
                Console.Error.WriteLine($"The options file cannot be located at: {optionsFilePath}");
                return ExitCode.OptionsError;
            }

            Console.WriteLine("Deleting issues...");

            using (var converter = new Converter(options))
            {
                await converter.DeleteAllIssues();
            }

            Console.WriteLine("Issues deleted.");

            return ExitCode.Success;
        }
#endif

        private static bool TryGetConverterOptions(string optionsFilePath, out ConverterOptions options)
        {
            options = null;

            if (!File.Exists(optionsFilePath))
                return false;

            var optionsData = File.ReadAllText(optionsFilePath);

            options = JsonConvert.DeserializeObject<ConverterOptions>(optionsData);

            return true;
        }
    }
}
