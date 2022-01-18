namespace Todo
{
    class Item
    {
        public int Id { get; internal set; }
        public string? Task { get; set; }
        public string? Note { get; set; }
        public string? Category { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; internal set; }
        public DateTime Deadline { get; set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime UpdatedAt { get; internal set; }
    }
}
