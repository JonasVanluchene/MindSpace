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
            var journalEntry = await _dbContext.JournalEntries.Include(j => j.JournalEntryTags).FirstOrDefaultAsync(j => j.Id == id);
            return journalEntry;
        }


        public async Task<JournalEntry?> Create(JournalEntry journalEntry)
        {
            await _dbContext.JournalEntries.AddAsync(journalEntry);
            await _dbContext.SaveChangesAsync();
            return journalEntry;
        }

        public async Task<JournalEntry?> Update(int id, JournalEntry updatedData, List<int> selectedTagIds)
        {
            // Fetch the existing JournalEntry from the database, including its related tags
            var dbEntry = await _dbContext.JournalEntries
                .Include(j => j.JournalEntryTags)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (dbEntry == null)
                return null;

            // TODO: Replace 'updatedData' with a dedicated DTO (e.g., JournalEntryUpdateDto)
            // This will help decouple service layer from domain entities and improve maintainability.

            // Update basic fields from the input (to be replaced by DTO properties)
            dbEntry.Title = updatedData.Title;
            dbEntry.Content = updatedData.Content;
            dbEntry.Mood = updatedData.Mood;

            // Clear existing tags and update with new ones based on selectedTagIds
            // TODO: Consider including SelectedTagIds inside the DTO to avoid separate parameters
            dbEntry.JournalEntryTags.Clear();
            foreach (var tagId in selectedTagIds)
            {
                dbEntry.JournalEntryTags.Add(new JournalEntryTag
                {
                    JournalEntryId = id,
                    TagId = tagId
                });
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Return the updated entity (or consider returning a DTO instead)
            return dbEntry;
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
