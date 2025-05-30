namespace MindSpace.Presentation.Mvc.Models.JournalEntry
{
    public class UserJournalEntriesViewModel
    {
        public IList<UserJournalEntryViewModel> JournalEntriesViewModels { get; set; } =
            new List<UserJournalEntryViewModel>();
    }
}
