using System.CommandLine;
using System.Xml.Linq;

namespace Todo
{
    public class Program
    {
        private static readonly string _xmlFilePath = @"./todo.xml";

        static async Task<int> Main(params string[] args)
        {
            #region RootCommand
            RootCommand rootCommand = new(description: SubCommand.Description.Root);
            #endregion

            #region Subcommand: Get
            Option<string?> getId = new(
                aliases: SubCommand.Alias.Id, 
                description: SubCommand.Description.GetId
            );
            Option<string?> getCategory = new(
                aliases: SubCommand.Alias.Category, 
                description: SubCommand.Description.GetCategory
            );
            Option<string?> getPriority = new(
                aliases: SubCommand.Alias.Priority, 
                description: SubCommand.Description.GetPriority
            );
            Option<string?> getStatus = new(
                aliases: SubCommand.Alias.Status, 
                description: SubCommand.Description.GetStatus
            );

            Command subCommandGet = new(name: SubCommand.Name.Get, description: SubCommand.Description.Get)
            {
                getId,
                getCategory,
                getPriority,
                getStatus,
            };

            subCommandGet.SetHandler(
                (string id, string category, string priority, string status) =>
                {
                    _xmlFilePath.Init();

                    var xml = XElement.Load(_xmlFilePath);
                    var items = xml.Get(id, category, priority, status);
                    
                    items.ShowItemNotFoundMessage(SubCommand.Name.Get);

                    foreach (var item in items.ToList())
                    {
                        item.Show();
                    }
                },
                getId,
                getCategory,
                getPriority,
                getStatus
            );

            rootCommand.AddCommand(subCommandGet);
            #endregion

            #region SubCommand: Add
            Option<string?> addTask = new(
                aliases: SubCommand.Alias.Task, 
                description: SubCommand.Description.AddTask
            );
            Option<string?> addNote = new(
                aliases: SubCommand.Alias.Note, 
                description: SubCommand.Description.AddNote, 
                getDefaultValue: () => null
            );
            Option<string?> addCategory = new(
                aliases: SubCommand.Alias.Category, 
                description: SubCommand.Description.AddCategory, 
                getDefaultValue: () => null
            );
            Option<string?> addDeadline = new(
                aliases: SubCommand.Alias.Deadline, 
                description: SubCommand.Description.AddDeadline, 
                getDefaultValue: () => DateTime.Now.AddDaysDefault().ToString()
            );
            Option<string?> addPriority = new(
                aliases: SubCommand.Alias.Priority, 
                description: SubCommand.Description.AddPriority, 
                getDefaultValue: () => "Medium"
            );

            Command subCommandAdd = new(name: SubCommand.Name.Add, description: SubCommand.Description.Add)
            {
                addTask,
                addNote,
                addCategory,
                addDeadline,
                addPriority,
            };

            subCommandAdd.SetHandler(
                (string task, string note, string category, string deadline, string priority) =>
                {
                    _xmlFilePath.Init();

                    var xml = XElement.Load(_xmlFilePath);
                    var item = xml.GetNextId().Add(task, note, category, deadline, priority);

                    item.Reload();
                    xml.Add(item);
                    item.Show();
                    xml.Save(_xmlFilePath);
                },
                addTask,
                addNote,
                addCategory,
                addDeadline,
                addPriority
            );

            rootCommand.AddCommand(subCommandAdd);
            #endregion

            #region SubCommand: Delete
            Option<string?> deleteId = new(
                aliases: SubCommand.Alias.Id, 
                description: SubCommand.Description.DeleteId
            );
            Option<string?> deleteCategory = new(
                aliases: SubCommand.Alias.Category, 
                description: SubCommand.Description.DeleteCategory
            );
            Option<string?> deletePriority = new(
                aliases: SubCommand.Alias.Priority, 
                description: SubCommand.Description.DeletePriority
            );
            Option<string?> deleteStatus = new(
                aliases: SubCommand.Alias.Status, 
                description: SubCommand.Description.DeleteStatus
            );

            Command subCommandDelete = new(name: SubCommand.Name.Delete, description: SubCommand.Description.Delete)
            {
                deleteId,
                deleteCategory,
                deletePriority,
                deleteStatus,
            };

            subCommandDelete.SetHandler(
                (string id, string category, string priority, string status) =>
                {
                    _xmlFilePath.Init();

                    var xml = XElement.Load(_xmlFilePath);
                    var items = xml.Get(id, category, priority, status);

                    items.ShowItemNotFoundMessage(SubCommand.Name.Delete);

                    SubCommand.Name.Delete.WaitConfirmation();

                    foreach (var item in items.ToList())
                    {
                        item.Delete();
                    }

                    xml.Save(_xmlFilePath);
                },
                deleteId,
                deleteCategory,
                deletePriority,
                deleteStatus
            );

            rootCommand.AddCommand(subCommandDelete);
            #endregion

            #region SubCommand: Update
            Option<string?> updateId = new(
                aliases: SubCommand.Alias.Id, 
                description: SubCommand.Description.UpdateId
            );
            Option<string?> updateTask = new(
                aliases: SubCommand.Alias.Task, 
                description: SubCommand.Description.UpdateTask
            );
            Option<string?> updateNote = new(
                aliases: SubCommand.Alias.Note, 
                description: SubCommand.Description.UpdateNote
            );
            Option<string?> updateCategory = new(
                aliases: SubCommand.Alias.Category, 
                description: SubCommand.Description.UpdateCategory
            );
            Option<string?> updateDeadline = new(
                aliases: SubCommand.Alias.Deadline, 
                description: SubCommand.Description.UpdateDeadline
            );
            Option<string?> updatePriority = new(
                aliases: SubCommand.Alias.Priority, 
                description: SubCommand.Description.UpdatePriority
            );
            Option<string?> updateStatus = new(
                aliases: SubCommand.Alias.Status, 
                description: SubCommand.Description.UpdateStatus
            );

            Command subCommandUpdate = new(name: SubCommand.Name.Update, description: SubCommand.Description.Update)
            {
                updateId,
                updateTask,
                updateNote,
                updateCategory,
                updateDeadline,
                updatePriority,
                updateStatus,
            };

            subCommandUpdate.SetHandler(
                (string id, string task, string note, string category, string deadline, string priority, string status) =>
                {
                    _xmlFilePath.Init();

                    var xml = XElement.Load(_xmlFilePath);
                    var items = xml.Elements(nameof(Item));
                    
                    items.ShowItemNotFoundMessage(SubCommand.Name.Update);

                    if (id != null) items = items.Where(x => x.GetValue(nameof(Item.Id)) == id);

                    foreach (var item in items.ToList())
                    {
                        item.Update(task, note, category, deadline, priority, status);
                        item.Reload();
                        item.Show();
                    }

                    xml.Save(_xmlFilePath);
                },
                updateId,
                updateTask,
                updateNote,
                updateCategory,
                updateDeadline,
                updatePriority,
                updateStatus
            );

            rootCommand.AddCommand(subCommandUpdate);
            #endregion

            #region SubCommand: Reload
            Command subCommandReload = new(
                name: SubCommand.Name.Reload, 
                description: SubCommand.Description.Reload
            );

            subCommandReload.SetHandler(
                () =>
                {
                    _xmlFilePath.Init();

                    var xml = XElement.Load(_xmlFilePath);
                    var items = xml.Elements(nameof(Item));

                    items.ShowItemNotFoundMessage(SubCommand.Name.Reload);

                    foreach (var item in items.ToList())
                    {
                        item.Reload();
                        item.Show();
                    }

                    xml.Save(_xmlFilePath);
                }
            );

            rootCommand.AddCommand(subCommandReload);
            #endregion

            return await rootCommand.InvokeAsync(args);
        }
    }
}
