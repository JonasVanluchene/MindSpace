namespace MindSpace.Presentation.Mvc.Models.JournalEntry
{
    public class UserJournalEntryViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string  Date { get; set; }

        public string Mood { get; set; }

        public List<string> Tags { get; set; } = new();
    }
}
