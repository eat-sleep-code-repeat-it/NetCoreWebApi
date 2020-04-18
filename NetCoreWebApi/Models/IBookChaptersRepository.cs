using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApi.Models
{
    public interface IBookChaptersRepository
    {
        Task InitAsync();
        Task AddAsync(BookChapter chapter);
        Task<BookChapter> RemoveAsync(Guid id);
        Task<IEnumerable<BookChapter>> GetAllAsync();
        Task<BookChapter> FindAsync(Guid id);
        Task UpdateAsync(BookChapter chapter);
    }
}
