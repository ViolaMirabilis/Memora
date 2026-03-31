using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;

namespace SimpleAUTH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardFolderController : BaseController
    {
        private readonly IFlashcardFolderService _flashcardFolderService;

        public FlashcardFolderController(IFlashcardFolderService flashcardFolderService)
        {
            _flashcardFolderService = flashcardFolderService;
        }

        [HttpGet]
        public ActionResult<List<FlashcardFolder>> GetAllFlashcardFolders()
        {
            int userId = CurrentUserId;
            return Ok(_flashcardFolderService.GetAllFlashcardFolders(userId));
        }

        [HttpGet("{id}")]
        public ActionResult<FlashcardFolder> GetFlashcardFolderById(int id)
        {
            int userId = CurrentUserId;
            var result = _flashcardFolderService.GetFlashcardFolderById(userId, id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public ActionResult<FlashcardFolder> UpdateFlashcardFolder(int id, FlashcardFolder updatedflashcardFolder)
        {
            int userId = CurrentUserId;
            var result = _flashcardFolderService.UpdateFlashcardFolder(userId, id, updatedflashcardFolder);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<FlashcardFolder> CreateFlashcardFolder(FlashcardFolder flashcardFolder)
        {
            int userId = CurrentUserId;
            return Ok(_flashcardFolderService.CreateFlashcardFolder(userId, flashcardFolder));
        }
    }
}
