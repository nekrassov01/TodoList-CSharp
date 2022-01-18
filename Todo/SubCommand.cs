namespace Todo
{
    static class SubCommand
    {
        public static class Name
        {
            public static readonly string Get = "get";
            public static readonly string Add = "add";
            public static readonly string Delete = "delete";
            public static readonly string Update = "update";
            public static readonly string Reload = "reload";
        }

        public static class Alias
        {
            public static readonly string[] Id = new string[] { "--id", "-i" };
            public static readonly string[] Task = new string[] { "--task", "-t" };
            public static readonly string[] Note = new string[] { "--note", "-n" };
            public static readonly string[] Category = new string[] { "--category", "-c" };
            public static readonly string[] Priority = new string[] { "--priority", "-p" };
            public static readonly string[] Deadline = new string[] { "--deadline", "-d" };
            public static readonly string[] Status = new string[] { "--status", "-s" };
        }

        public static class Description
        {
            public static readonly string Root = "A simple application to manage todo list on the command line";
            public static readonly string Get = "Get the todo list";
            public static readonly string GetId = "Specify the id to filter when getting todo items";
            public static readonly string GetCategory = "Specify the category to filter when getting todo items";
            public static readonly string GetPriority = "Specify the priority to filter when getting todo items";
            public static readonly string GetStatus = "Specify the status to filter when getting todo items";
            public static readonly string Add = "Add a item to the todo list";
            public static readonly string AddTask = "Specify the heading when adding the todo item";
            public static readonly string AddNote = "Specify the detailed information when adding the todo item";
            public static readonly string AddCategory = "Specify the free-text category when adding the todo item";
            public static readonly string AddPriority = "Specify the priority when adding the todo item: 'High', Low, and Medium";
            public static readonly string AddDeadline = "Specify the deadline (used to update status) when adding the todo item";
            public static readonly string Delete = "Delete todo items from the todo list";
            public static readonly string DeleteId = "Specify the id to filter when deleting todo items";
            public static readonly string DeleteCategory = "Specify the category to filter when deleting todo items";
            public static readonly string DeletePriority = "Specify the priority to filter when deleting todo items";
            public static readonly string DeleteStatus = "Specify the status to filter when deleting todo items";
            public static readonly string Update = "Update todo items in the todo list";
            public static readonly string UpdateId = "Specify the id to filter the todo items";
            public static readonly string UpdateTask = "Specify the heading when updating the todo item";
            public static readonly string UpdateNote = "Specify the detailed information when updating the todo item";
            public static readonly string UpdateCategory = "Specify the free-text category when updating the todo item";
            public static readonly string UpdatePriority = "Specify the priority when updating the todo item: High, Low, Medium";
            public static readonly string UpdateDeadline = "Specify the deadline (used to update the status) when updating the todo item";
            public static readonly string UpdateStatus = "Specify the status when updating the todo item: Open, and Closed";
            public static readonly string Reload = "Update the status of todo items by comparing their deadlines with the current date";
        }
    }
}
