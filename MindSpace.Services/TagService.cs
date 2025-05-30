using MindSpace.Model;
using MindSpace.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MindSpace.Services
{
    public class TagService
    {
        private readonly MindSpaceDbContext _dbContext;

        public TagService(MindSpaceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Tag>> Find()
        {
            return await _dbContext.Tags.ToListAsync();
        }

        public async Task<Tag?> Create(Tag tag)
        {
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();
            return tag;
        }
    }
}
