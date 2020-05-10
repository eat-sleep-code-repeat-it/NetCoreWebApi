
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreWebApi.Models;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;

namespace NetCoreWebApi.Controllers
{
    /// <summary>
    /// ODataController
    /// </summary>
    
    // !!! need this line to make swagger work
    [ApiExplorerSettings(IgnoreApi = true)] 
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ODataController : ControllerBase
    {
        private readonly BooksContext _context;

        /// <summary>
        /// BooksController
        /// </summary>
        /// <param name="repository"></param>
        public ODataController(BooksContext context)
        {
            _context = context;
        }
        // GET: api/Books
        [EnableQuery(PageSize = 50)]
        [ODataRoute]
        public IQueryable<BookChapter> Get()
        {
            return _context.Chapters;
        }
    }
}
