using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;
using SimpleAUTH.DTO;

namespace SimpleAUTH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardController : BaseController
    {
        private readonly IFlashcardService _flashcardService;

        public FlashcardController(IFlashcardService flashcardService)
        {
            _flashcardService = flashcardService;
        }


        [HttpGet]
        public async Task<ActionResult<List<FlashcardDTO>>> GetAllUserFlashcards()
        {
            int userId = CurrentUserId;         // taken from the abstract class
            var existingFlashcards = await _flashcardService.GetAllUserFlashcards(userId);

            if (existingFlashcards.Count == 0)
                return Ok(new List<FlashcardDTO>());        // returns empty list

            var result = existingFlashcards.Select(flashcard => new FlashcardDTO
            {
                Id = flashcard.Id,
                FlashcardSetId = flashcard.FlashcardSetId,
                Front = flashcard.Front,
                Back = flashcard.Back
            })
            .ToList();

            return Ok(result);
        }

        // CHANGE HERE OR "GET SHARED SET BY ID"
        // returns existing flashcards from a set only for the currently logged in user
        [HttpGet("set/{setId}")]
        public async Task<ActionResult<List<FlashcardDTO>>> GetFlashcardsFromSet(int setId)
        {
            int userId = CurrentUserId;
            var existingFlashcards = await _flashcardService.GetFlashcardsFromSet(userId, setId);

            if (existingFlashcards.Count == 0)
                return Ok(new List<FlashcardDTO>());        // returns empty list

            var result = existingFlashcards.Select(flashcard => new FlashcardDTO
            {
                Id = flashcard.Id,
                FlashcardSetId = flashcard.FlashcardSetId,
                Front = flashcard.Front,
                Back = flashcard.Back
            })
            .ToList();

            return Ok(result);

        }

        // returns existing flashcars from a set that has been shared
        [HttpGet("/set/{setId}/shared")]
        public async Task<ActionResult<List<FlashcardDTO>>> GetSharedFlashcardsFromSet(int setId)
        {
            var existingFlashcards = await _flashcardService.GetSharedFlashcardsFromSet(setId);

            if (existingFlashcards.Count == 0)
                return Ok(new List<FlashcardDTO>());        // returns empty list

            var result = existingFlashcards.Select(flashcard => new FlashcardDTO
            {
                Id = flashcard.Id,
                FlashcardSetId = flashcard.FlashcardSetId,
                Front = flashcard.Front,
                Back = flashcard.Back
            })
            .ToList();

            return Ok(result);
        }



        [HttpGet("{id}")]
        public ActionResult<Flashcard> GetFlashcardById(int id)
        {
            int userId = CurrentUserId;
            var result = _flashcardService.GetFlashcardById(userId, id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<FlashcardDTO>> CreateFlashcard(FlashcardDTO dto)
        {
            int userId = CurrentUserId;

            // mapping DTO to entity
            var existingFlashcard = new Flashcard
            {
                Id = dto.Id,
                FlashcardSetId = dto.FlashcardSetId,
                Front = dto.Front,
                Back = dto.Back
            };

            var created = await _flashcardService.CreateFlashcard(userId, existingFlashcard);

            var result = new FlashcardDTO
            {
                Id = created.Id,
                FlashcardSetId = created.FlashcardSetId,
                Front = created.Front,
                Back = created.Back
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult<Flashcard> UpdateFlashcard(int id, Flashcard updatedFlashcard)
        {
            int userId = CurrentUserId;
            var result = _flashcardService.UpdateFlashcard(userId, id, updatedFlashcard);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public ActionResult<string> DeleteFlashcard(int id)
        {
            int userId = CurrentUserId;
            var result = _flashcardService.DeleteFlashcard(userId, id);
            if (result == null)
                return NotFound();

            return Ok(result);

        }

        // overwrite / update ALL flashcards
        [HttpPost("/Copy")]
        public async Task<IActionResult> CopyFromSharedSet(int id, CopyFlashcardsToNewSet dto)
        {
            int userId = CurrentUserId;
            // awaits the method. It has no return, so just awaiting without assigning
            await _flashcardService.CopyFlashcardsFromSharedSet(userId, dto);

            return NoContent();
        }
    }
}
