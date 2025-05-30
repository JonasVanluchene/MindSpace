using MindSpace.Model;
using MindSpace.Services;

namespace MindSpace.Presentation.Mvc.Helpers.Database
{
    public class SeedDatabaseHelper
    {
        public static async Task SeedPredefinedTags(TagService tagService)
        {
            var tags = await tagService.Find();
            if (!tags.Any())
            {
                var predefinedTags = new[]
                {
                    "Work", "Personal", "Reflection", "Goals", "Gratitude",
                    "Relationships", "Health", "Travel", "Learning", "Creativity",
                    "Family", "Friends", "Exercise", "Meditation", "Progress"
                };

                foreach (var tagName in predefinedTags)
                {
                    await tagService.Create(new Tag
                    {
                        Name = tagName
                    });
                }

                
            }
        }
    }
}
