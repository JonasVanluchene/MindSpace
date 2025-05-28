using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSpace.Model
{
    [Table(nameof(Tag))]
    public class Tag
    {

        public int Id { get; set; }
        public required string Name { get; set; }

        public List<JournalEntryTag> JournalEntryTags { get; set; } = new();
    }
}
