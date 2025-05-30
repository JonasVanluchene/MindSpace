using MindSpace.Model;
using System.ComponentModel.DataAnnotations;

namespace MindSpace.Presentation.Mvc.Models.JournalEntry
{
    public class EditViewModel
    {
        [Required]
        public string? Title { get; set; }

        public string? Content { get; set; }

        [Required]
        public Mood Mood { get; set; }

        public List<int> SelectedTagIds { get; set; } = new();
        public string? NewTagName { get; set; }

        // For display
        public IList<Tag> AvailableTags { get; set; } = new List<Tag>();
    }
}
