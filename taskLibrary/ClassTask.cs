
namespace taskLibrary
{
    public class ClassTask
    {
        public int id { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateAndTime { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsNewlyRegistered { get; set; } = false;

    }
}
