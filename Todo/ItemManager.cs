using System.Xml.Linq;

namespace Todo
{
    static class ItemManager
    {
        private static readonly int _intervalWarning = 1;
        private static readonly int _columnWidth = 10;
        private static readonly DateTime _now = DateTime.Now.TruncateMillisecond();

        public static void Init(this string xmlFilePath)
        {
            if (File.Exists(xmlFilePath)) return;

            var xml = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement("Items")
            );
            xml.Save(xmlFilePath);
        }

        public static IEnumerable<XElement> Get(this XElement xml, string id, string category, string priority, string status)
        {
            Dictionary<string, string?> elements = new()
            {
                { nameof(Item.Id), id },
                { nameof(Item.Category), category },
                { nameof(Item.Priority), priority.ToTitleCase() },
                { nameof(Item.Status), status.ToTitleCase() }
            };

            var items = xml.Elements(nameof(Item));

            foreach (var element in elements)
            {
                if (element.Value == null) continue;
                items = items?.Where(x => x?.GetValue(element.Key) == element.Value);
            }

            return items;
        }

        public static void Show(this XElement? item)
        {
            Console.ForegroundColor = item?.GetValue(nameof(Item.Status)).ToStatus() switch
            {
                Status.Open => ConsoleColor.Gray,
                Status.Closed => ConsoleColor.DarkGray,
                Status.Warning => ConsoleColor.DarkYellow,
                Status.Expired => ConsoleColor.DarkRed,
                _ => ConsoleColor.Gray,
            };

            Console.WriteLine();

            foreach (var property in typeof(Item).GetProperties())
            {
                string padding = new(' ', _columnWidth - property.Name.Length);
                Console.WriteLine($"{property.Name}{padding}| {item?.GetValue(property.Name)}");
            }

            Console.ResetColor();
        }

        public static XElement Add(this int id, string? task, string? note, string? category, string? deadline, string? priority)
        {
            if (task == null) ItemHelper.ExitError(ErrorCode.TaskNotObtained, $"\"{task}\" can not be obtained");

            XElement item = new(nameof(Item),
                new XAttribute(nameof(Item.Id), id),
                new XElement(nameof(Item.Task), task),
                new XElement(nameof(Item.Note), note),
                new XElement(nameof(Item.Category), category),
                new XElement(nameof(Item.Priority), priority.ToPriority()),
                new XElement(nameof(Item.Status), Status.Open),
                new XElement(nameof(Item.Deadline), deadline.ToDateTime()),
                new XElement(nameof(Item.CreatedAt), _now),
                new XElement(nameof(Item.UpdatedAt), _now)
            );

            return item;
        }

        public static void Delete(this XElement? item)
        {
            item?.Remove();
            Console.WriteLine($"{SubCommand.Name.Delete.ToTitleCase()}: Id {item?.GetValue(nameof(Item.Id))} is deleted");
        }

        public static void Update(this XElement? item, string? task, string? note, string? category, string? deadline, string? priority, string? status)
        {
            Dictionary<string, string?> elements = new()
            {
                { nameof(Item.Task), task },
                { nameof(Item.Note), note },
                { nameof(Item.Category), category },
                { nameof(Item.Deadline), deadline },
                { nameof(Item.Priority), priority },
                { nameof(Item.Status), status },
            };
            
            foreach (var element in elements)
            {
                if (element.Value == null) continue;
            
                object value = element.Key switch
                {
                    nameof(Item.Deadline) => element.Value.ToDateTime(),
                    nameof(Item.Priority) => element.Value.ToPriority(),
                    nameof(Item.Status) => element.Value.ValidateManualUpdateAllowed(),
                    _ => element.Value
                };
                item?.Element(element.Key)?.SetValue(value);
                item?.Element(nameof(Item.UpdatedAt))?.SetValue(_now);
            }
        }

        public static void Reload(this XElement item)
        {
            Status originalStatus = item.GetValue(nameof(Item.Status)).ToStatus();
            DateTime deadline = item.GetValue(nameof(Item.Deadline)).ToDateTime();
            TimeSpan currentTimespan = deadline - _now;
            TimeSpan warningTimespan = TimeSpan.FromDays(_intervalWarning);

            if (originalStatus == Status.Closed) return;

            Status expectedStatus = currentTimespan <= warningTimespan
                ? Status.Warning
                : Status.Open;

            expectedStatus = deadline <= _now
                ? Status.Expired
                : expectedStatus;

            if (expectedStatus == originalStatus) return;

            item.Element(nameof(Item.Status))?.SetValue(expectedStatus);
            item.Element(nameof(Item.UpdatedAt))?.SetValue(_now);
        }
    }
}
