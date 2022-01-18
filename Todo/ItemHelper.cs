using System.Globalization;
using System.Xml.Linq;

namespace Todo
{
    static class ItemHelper
    {
        private static readonly int _intervalDefault = 7;

        public static void ExitError(ErrorCode code, string? message)
        {
            int num = (int)code;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Error.WriteLine($"Error({num}): {message}");
            Console.ResetColor();
            Environment.Exit(num);
        }

        public static DateTime AddDaysDefault(this DateTime datetime)
        {
            return TruncateMillisecond(datetime.AddDays(_intervalDefault));
        }

        public static DateTime TruncateMillisecond(this DateTime datetime)
        {
            return datetime.AddTicks(-(datetime.Ticks % TimeSpan.TicksPerSecond));
        }

        public static string? ToTitleCase(this string? str)
        {
            return str != null ? String.Concat(char.ToUpper(str[0]), str.Substring(1).ToLower()) : null;
        }

        public static DateTime ToDateTime(this string? str)
        {
            if (!DateTime.TryParse(str, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out DateTime datetime))
                ExitError(ErrorCode.DateTimeNotResolved, $"\"{str}\" can not be resolved to {typeof(DateTime)}");
            return datetime;
        }

        public static Priority ToPriority(this string? str)
        {
            if (!Enum.TryParse(str, ignoreCase: true, out Priority priority))
                ExitError(ErrorCode.PriorityNotResolved, $"\"{str}\" can not be resolved to {typeof(Priority)}");
            return priority;
        }
        public static Status ToStatus(this string? str)
        {
            if (!Enum.TryParse(str, ignoreCase: true, out Status status))
                ExitError(ErrorCode.StatusNotResolved, $"\"{str}\" can not be resolved to {typeof(Status)}");
            return status;
        }

        public static Status ValidateManualUpdateAllowed(this string? str)
        {
            Status status = str.ToStatus();
            List<Status> manualStatus = new() { Status.Open, Status.Closed };
            if (!manualStatus.Contains(status))
                ExitError(ErrorCode.StatusNotAllowed, $"\"{status}\" is not available for manual updates");
            return status;
        }

        public static string? GetValue(this XElement item, string str)
        {
            return str == nameof(Item.Id)
                ? item.Attribute(str)?.Value
                : item.Element(str)?.Value;
        }

        public static int GetNextId(this XElement xml)
        {
            return Convert.ToInt32(xml.Elements(nameof(Item))
                ?.Select(x => x.GetValue(nameof(Item.Id)))
                ?.LastOrDefault() ?? "0") + 1;
        }

        public static void ShowItemNotFoundMessage(this IEnumerable<XElement?> items, string? subCommand)
        {
            if (!items.Any())
            {
                Console.WriteLine($"{subCommand.ToTitleCase()}: No matching items");
                Environment.Exit(0);
            }
        }

        public static void WaitConfirmation(this string? subCommand)
        {
            string? action = subCommand.ToTitleCase();
            Console.Write($"{action}? (y/n): ");
            string? answer = Console.ReadLine() ?? String.Empty;
            List<string> positive = new() { "y", "Y", "yes", "Yes" };

            if (!positive.Contains(answer))
            {
                Console.WriteLine($"{action}: Canceled");
                Environment.Exit(0);
            }
        }
    }
}
