using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAUTH.Interfaces;
using SimpleAUTH.Models;
using SimpleAUTH.DTO;
using System.Reflection.Metadata.Ecma335;

namespace SimpleAUTH.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardSetController : BaseController
    {
        private readonly IFlashcardSetService _flashcardSetService;
        public FlashcardSetController(IFlashcardSetService flashcardSetService)
        {
            _flashcardSetService = flashcardSetService;
        }

        [HttpGet]
        public async Task<ActionResult<List<FlashcardSetDTO>>> GetAllFlashcardSets()
        {
            int userId = CurrentUserId; // gets it from the BaseController abstract class
            var existingFlashcardSets = await _flashcardSetService.GetAllFlashcardSets(userId);

            if (existingFlashcardSets.Count == 0)
                return Ok(new List<FlashcardSetDTO>());     // returning an empty list instead of 404 not found

            // maps the original list to a list of DTOs
            var result = existingFlashcardSets.Select(set => new FlashcardSetDTO
            {
                Id = set.Id,
                Name = set.Name,
                FolderId = set.FolderId,
                FolderName = set.Folder?.Name,
                IsSharing = set.IsSharing,
                SharingCode = set.SharingCode
            }) 
            .ToList();


            return Ok(result);
        }

        
        [HttpGet("{id}/setById")]
        public async Task<ActionResult<FlashcardSetDTO>> GetFlashcardSetById(int id)
        {
            int userId = CurrentUserId;
            var existingFlashcard = await _flashcardSetService.GetFlashcardSetById(userId, id);

            // checks if flashcard exists
            if (existingFlashcard == null)
                return NotFound();

            // mapping the original to DTO
            var result = new FlashcardSetDTO
            {
                Id = existingFlashcard.Id,
                Name = existingFlashcard.Name,
                FolderId = existingFlashcard.FolderId,
                FolderName = existingFlashcard.Folder?.Name,    // checking for null, because folder is most likely empty
            };
            

            return Ok(result);
        }


        /// <summary>
        ///  we pass in the DTO, it creates a new set and sends back a DTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<FlashcardSetDTO>> CreateFlashcardSet(FlashcardSetDTO dto)
        {
            int userId = CurrentUserId;

            // mapping a flashcard set with DTO's values
            var existingSet = new FlashcardSet
            {
                Id = dto.Id,
                Name = dto.Name,
                FolderId = dto.FolderId
            };

            // pushing it to the service
            var created = await _flashcardSetService.CreateFlashcardSet(userId, existingSet);

            var result = new FlashcardSetDTO
            {
                Id = created.Id,
                Name = created.Name,
                FolderId = created.Id,
                FolderName = created.Folder?.Name
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FlashcardSet>> UpdateFlashcardSet(int id, FlashcardSet updatedFlashcardSet)
        {
            int userId = CurrentUserId;
            var result = _flashcardSetService.UpdateFlashcardSet(userId, id, updatedFlashcardSet);
            if (result == null)
                return NotFound();

            return Ok(result);  
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteFlashcardSet(int id)
        {
            int userId = CurrentUserId;
            var result = await _flashcardSetService.DeleteFlashcardSet(userId, id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // Updating the FlashcardSet name
        [HttpPatch]
        public async Task<ActionResult<bool>> UpdateFlashcardSetName(UpdatedNameFlashcardSetDTO dto)
        {
            int userId = CurrentUserId;
            int setId = dto.Id;
            var result = await _flashcardSetService.UpdateFlashcardSetName(userId, setId, dto);
            if (!result)
                return false;

            return Ok(result); 
        }

        // shares the flashcard set
        [HttpPost("{id}/sharing")]
        public async Task<ActionResult<bool>> ShareFlashcardSet(int id)
        {
            int userId = CurrentUserId;
            var result = await _flashcardSetService.ShareFlashcardSet(userId, id);
            if (!result)
                return false;

            return Ok(result);
        }

        // retrieves the "sharing code"
        [HttpGet("{id}/sharing-code")]
        public async Task<ActionResult<string>> GetSharingCodeFlashcardSet(int id)
        {
            int userId = CurrentUserId;
            var result = await _flashcardSetService.GetSharingCodeFlashcardSet(userId, id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // gets the flashcard set by sending in the code
        [HttpGet("shared/{code}")]
        public async Task<ActionResult<FlashcardSet>> GetBySharingCodeFlashcardSet(string code)
        {
            var existingFlashcardSet = await _flashcardSetService.GetFlashcardSetBySharingCode(code);

            if (existingFlashcardSet == null)
                return NotFound();

            // mapping the original to DTO
            var result = new FlashcardSetDTO
            {
                Id = existingFlashcardSet.Id,
                Name = existingFlashcardSet.Name,
                FolderId = existingFlashcardSet.FolderId,
                FolderName = existingFlashcardSet.Folder?.Name,    // checking for null, because folder is most likely empty
                IsSharing = existingFlashcardSet.IsSharing,
                SharingCode = existingFlashcardSet.SharingCode
            };

            return Ok(result);
        }

        // gets the last added flashcard for a given user
        [HttpGet("/last-added")]
        public async Task<ActionResult<FlashcardSet>> GetLastFlashcardSet()
        {
            int userId = CurrentUserId;
            var existingFlashcardSet = await _flashcardSetService.GetLastFlashcardSet(userId);

            if (existingFlashcardSet == null)
                return Ok(new List<FlashcardSetDTO>());     // returning an empty list instead of 404 not found

            // maps the original list to a new DTO
            var result = new FlashcardSetDTO
            {
                Id = existingFlashcardSet.Id,
                Name = existingFlashcardSet.Name,
                FolderId = existingFlashcardSet.FolderId,
                FolderName = existingFlashcardSet.Folder?.Name,
                IsSharing = existingFlashcardSet.IsSharing,
                SharingCode = existingFlashcardSet.SharingCode
            };


            return Ok(result);
        }
    }
}
