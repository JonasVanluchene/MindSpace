using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MindSpace.Model;
using MindSpace.Repository;

namespace MindSpace.Services
{
    public class JournalEntryService
    {
        private readonly MindSpaceDbContext _dbContext;

        public JournalEntryService(MindSpaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<List<JournalEntry>> Find(string userId)
        {
            return await _dbContext.JournalEntries
                .Where(j => j.UserId == userId)
                .OrderByDescending(j => j.Date)
                .Include(j => j.JournalEntryTags)
                .ThenInclude(jt => jt.Tag) // Optional: load actual tag names if using a join entity
                .ToListAsync();
        }


        public async Task<JournalEntry?> Get(int id)
        {
            var journalEntry = await _dbContext.JournalEntries.FirstOrDefaultAsync(b => b.Id == id);
            return journalEntry;
        }


        public async Task<JournalEntry?> Create(JournalEntry journalEntry)
        {
            await _dbContext.JournalEntries.AddAsync(journalEntry);
            await _dbContext.SaveChangesAsync();
            return journalEntry;
        }

        public async Task<JournalEntry?> Update(int id, JournalEntry journalEntry)
        {
            var dbJournalEntry = await Get(id);
            if (dbJournalEntry is null)
            {
                return null;
            }

            dbJournalEntry.Title = journalEntry.Title;
            dbJournalEntry.Content = journalEntry.Content;
            dbJournalEntry.Mood = journalEntry.Mood;
            dbJournalEntry.JournalEntryTags = journalEntry.JournalEntryTags;


            await _dbContext.SaveChangesAsync();
            return dbJournalEntry;
        }

        public async Task Delete(int id)
        {
            var journalEntry = await Get(id);
            if (journalEntry is null)
            {
                return;
            }
            _dbContext.JournalEntries.Remove(journalEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
