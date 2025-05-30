using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSpace.Model;
using MindSpace.Presentation.Mvc.Models.JournalEntry;
using MindSpace.Services;
using System.Security.Claims;

namespace MindSpace.Presentation.Mvc.Controllers
{
    [Authorize] // Ensures only logged-in users can access this controller
    public class JournalEntriesController : Controller
    {
        private readonly JournalEntryService _journalEntryService;
        private readonly TagService _tagService;

        public JournalEntriesController(JournalEntryService journalEntryService, TagService tagService)
        {
            _journalEntryService = journalEntryService;
            _tagService = tagService;
        }

        /// <summary>
        /// Displays the list of journal entries for the currently logged-in user.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return NotFound();
            }

            var journalEntries = await _journalEntryService.Find(userId);

            // Map domain models to view models
            var viewModel = new UserJournalEntriesViewModel
            {
                JournalEntriesViewModels = journalEntries.Select(j => new UserJournalEntryViewModel
                {
                    Id = j.Id,
                    Title = j.Title,
                    Content = j.Content,
                    Date = j.Date.ToShortDateString(),
                    Mood = j.Mood.ToString(),
                    Tags = j.JournalEntryTags.Select(jt => jt.Tag.Name).ToList()
                }).ToList()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Displays details of a specific journal entry.
        /// </summary>
        [HttpGet]
        public IActionResult Details([FromRoute] int id)
        {
            return View(); // Not yet implemented
        }

        /// <summary>
        /// Shows the create journal entry form.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateViewModel()
            {
                AvailableTags = await _tagService.Find()
            };
            return View(viewModel);
        }

        /// <summary>
        /// Handles form submission to create a new journal entry.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Re-fetch available tags for redisplay
                viewModel.AvailableTags = await _tagService.Find();
                return View(viewModel);
            }

            // Create a new journal entry model from the view model
            var journalEntry = new JournalEntry
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Date = DateTime.Now,
                Title = viewModel.Title,
                Content = viewModel.Content,
                Mood = viewModel.Mood,
                JournalEntryTags = viewModel.SelectedTagIds
                    .Select(tagId => new JournalEntryTag { TagId = tagId })
                    .ToList()
            };

            await _journalEntryService.Create(journalEntry);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Shows the edit form for an existing journal entry.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            var journalEntry = await _journalEntryService.Get(id);

            // Populate view model with existing entry data and selected tag IDs
            var viewModel = new CreateViewModel
            {
                Title = journalEntry.Title,
                Content = journalEntry.Content,
                Mood = journalEntry.Mood,
                SelectedTagIds = journalEntry.JournalEntryTags
                    .Select(jet => jet.TagId) 
                    .ToList(),
                AvailableTags = await _tagService.Find()
            };

            return View(viewModel);
        }

        /// <summary>
        /// Handles form submission to update an existing journal entry.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] CreateViewModel viewModel)
        {
            // Check if the submitted form data is valid according to the model validation rules
            if (!ModelState.IsValid)
            {
                // If validation fails, repopulate the AvailableTags so the view can render correctly
                viewModel.AvailableTags = await _tagService.Find();

                // Return the view with the invalid model to show validation errors
                return View(viewModel);
            }

            // Prepare a JournalEntry domain model instance to hold updated data
            // Note: Currently, we do NOT map Tags here because they are handled separately via the SelectedTagIds list
            var updatedEntry = new JournalEntry
            {
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Set current user as owner
                Title = viewModel.Title,
                Content = viewModel.Content,
                Mood = viewModel.Mood
            };

            // Call service layer to update the journal entry with the new data and selected tags
            var updated = await _journalEntryService.Update(id, updatedEntry, viewModel.SelectedTagIds);

            // If no entry was found for the given id, return 404 Not Found
            if (updated == null)
                return NotFound();

            // On success, redirect user back to the journal entries list page
            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Handles journal entry deletion.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _journalEntryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}


