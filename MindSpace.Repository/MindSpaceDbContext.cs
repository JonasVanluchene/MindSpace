using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindSpace.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindSpace.Repository
{
    public class MindSpaceDbContext : IdentityDbContext<User>
    {
        public MindSpaceDbContext(DbContextOptions<MindSpaceDbContext> options) : base(options)
        {

        }

        
        public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();

        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<ReflectionQuestion> ReflectionQuestions => Set<ReflectionQuestion>();

        public DbSet<JournalEntryTag> JournalEntryTags => Set<JournalEntryTag>();



    }
}
