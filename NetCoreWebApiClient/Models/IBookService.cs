using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApiClient.Models
{
    public interface IBookService
    {
        Task<IEnumerable<BookChapter>> GetBookChapters();
    }
}
