using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApi.Models;

namespace NetCoreWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookChaptersController : ControllerBase
    {
        private readonly IBookChaptersRepository _repository;
        public BookChaptersController(IBookChaptersRepository repository)
        {
            _repository = repository;
        }

        // GET: api/bookchapters
        [HttpGet()]
        public Task<IEnumerable<BookChapter>> GetBookChaptersAsync() => _repository.GetAllAsync();

        // GET api/bookchapters/guid
        [HttpGet("{id}", Name = nameof(GetBookChapterByIdAsync))]
        public async Task<IActionResult> GetBookChapterByIdAsync(Guid id)
        {
            BookChapter chapter = await _repository.FindAsync(id);
            if (chapter == null)
            {
                return NotFound();
            }
            else
            {
                return new ObjectResult(chapter);
            }
        }

        // POST api/bookchapters
        [HttpPost]
        public async Task<IActionResult> PostBookChapterAsync([FromBody]BookChapter chapter)
        {
            if (chapter == null)
            {
                return BadRequest();
            }
            await _repository.AddAsync(chapter);
            // return a 201 response, Created
            return CreatedAtRoute(nameof(GetBookChapterByIdAsync), new { id = chapter.Id }, chapter);
        }

        // PUT api/bookchapters/guid
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookChapterAsync(Guid id, [FromBody]BookChapter chapter)
        {
            if (chapter == null || id != chapter.Id)
            {
                return BadRequest();
            }
            if (await _repository.FindAsync(id) == null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(chapter);
            return new NoContentResult();  // 204
        }

        // DELETE api/bookchapters/guid
        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.RemoveAsync(id);
            // void returns 204, No Content
        }
    }
}