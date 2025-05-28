using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSpace.Model
{

    [Table(nameof(JournalEntry))]
    public class JournalEntry
    {

        public int Id { get; set; }


        public required string Title { get; set; }

        public string? Content { get; set; }

        public DateTime Date { get; set; }

        public Mood Mood { get; set; }

        public int? ReflectionQuestionId { get; set; }
        public ReflectionQuestion? ReflectionQuestion { get; set; }

        public required string UserId { get; set; }
        public User User { get; set; }

        public List<JournalEntryTag> JournalEntryTags { get; set; } = new();
    }
}
