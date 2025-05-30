using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MindSpace.Model;
using MindSpace.Presentation.Mvc.Models.JournalEntry;
using MindSpace.Services;
using System.Security.Claims;

namespace MindSpace.Presentation.Mvc.Controllers
{
    [Authorize]
    public class JournalEntriesController : Controller
    {
        private readonly JournalEntryService _journalEntryService;
        private readonly TagService _tagService;



        public JournalEntriesController(JournalEntryService journalEntryService,TagService tagService)
        {
            _journalEntryService = journalEntryService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
            {
                return NotFound();
            }
            var journalEntries = await _journalEntryService.Find(userId);
            var viewModel = new UserJournalEntriesViewModel
            {
                JournalEntriesViewModels = journalEntries.Select(j => new UserJournalEntryViewModel
                {
                    Title = j.Title,
                    Content = j.Content,
                    Date = j.Date.ToShortDateString(),
                    Mood = j.Mood.ToString(),
                    Tags = j.JournalEntryTags.Select(jt => jt.Tag.Name).ToList()
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Details([FromRoute] int id)
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateViewModel()
            {
                AvailableTags = await _tagService.Find()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.AvailableTags = await _tagService.Find();
                return View(viewModel);
            }

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


        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, [FromForm] JournalEntry journalEntry)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete([FromRoute] int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
