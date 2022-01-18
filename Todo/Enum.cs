namespace Todo
{
    internal enum Priority
    {
        Medium,
        High,
        Low
    }

    internal enum Status
    {
        Open,
        Closed,
        Warning,
        Expired
    }

    internal enum ErrorCode
    {
        TaskNotObtained = 1,
        DateTimeNotResolved = 2,
        PriorityNotResolved = 3,
        StatusNotResolved = 4,
        StatusNotAllowed = 5,
    }
}
