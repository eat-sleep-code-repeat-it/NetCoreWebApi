using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreWebApi.Models;

namespace NetCoreWebApi.Controllers
{
    /// <summary>
    /// BookChaptersController
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BookChaptersController : ControllerBase
    {
        private readonly IBookChaptersRepository _repository;
        private readonly ILogger _logger;
        /// <summary>
        /// BookChaptersController
        /// </summary>
        /// <param name="repository"></param>
        public BookChaptersController(IBookChaptersRepository repository, ILogger<BookChaptersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// GetBookChaptersAsync
        /// </summary>
        /// <returns></returns>
        // GET: api/bookchapters
        [HttpGet()]
        public Task<IEnumerable<BookChapter>> GetBookChaptersAsync()
        {
            _logger.LogInformation("Retrieving book chapters.");
            return _repository.GetAllAsync();
        }

        /// <summary>
        /// GetBookChapterByIdAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/bookchapters/guid
        [HttpGet("{id}", Name = nameof(GetBookChapterByIdAsync))]
        public async Task<IActionResult> GetBookChapterByIdAsync(Guid id)
        {
            string message = string.Format($"Id={id.ToString()}");
            _logger.LogInformation(message);
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

        /// <summary>
        /// PostBookChapterAsync
        /// </summary>
        /// <param name="chapter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// PutBookChapterAsync
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chapter"></param>
        /// <returns></returns>
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

        /// <summary>
        /// DeleteAsync
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/bookchapters/guid
        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            await _repository.RemoveAsync(id);
            // void returns 204, No Content
        }
    }
}