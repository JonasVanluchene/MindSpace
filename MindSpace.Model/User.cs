using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MindSpace.Model
{
    [Table(nameof(User))]
    public class User : IdentityUser
    {
        
        public string? AvatarUrl { get; set; }

        public bool Newsletter { get; set; }


        public List<JournalEntry> JournalEntries { get; set; } = new();
    }
}
