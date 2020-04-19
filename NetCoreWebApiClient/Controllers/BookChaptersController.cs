using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreWebApiClient.Models;

namespace NetCoreWebApiClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookChaptersController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookChaptersController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: api/bookchapters
        [HttpGet()]
        public Task<IEnumerable<BookChapter>> GetBookChaptersAsync() 
        {
            return _bookService.GetBookChapters();
        }
    }
}