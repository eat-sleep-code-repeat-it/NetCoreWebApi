﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreWebApi.Models
{
    public class BookChaptersRepository : IBookChaptersRepository, IDisposable
    {
        private BooksContext _booksContext;
        private readonly ILogger _logger;
        public BookChaptersRepository(BooksContext booksContext, ILoggerFactory logFactory)
        {
            _booksContext = booksContext;
            _logger = logFactory.CreateLogger<BookChaptersRepository>();
            _logger.LogInformation("BookChaptersRepository Constructor");
        }

        public void Dispose()
        {
            _booksContext?.Dispose();
        }
        public async Task AddAsync(BookChapter chapter)
        {
            _logger.LogInformation("BookChaptersRepository AddAsync");
            _booksContext.Chapters.Add(chapter);
            await _booksContext.SaveChangesAsync();
        }


        public Task<BookChapter> FindAsync(Guid id) =>
            _booksContext.Chapters.SingleOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<BookChapter>> GetAllAsync() =>
            await _booksContext.Chapters.ToListAsync();


        public Task InitAsync()
        {
            return Task.FromResult<object>(null);
        }

        public async Task<BookChapter> RemoveAsync(Guid id)
        {
            BookChapter chapter = await _booksContext.Chapters.SingleOrDefaultAsync(c => c.Id == id);
            if (chapter == null) return null;

            _booksContext.Chapters.Remove(chapter);
            await _booksContext.SaveChangesAsync();
            return chapter;
        }

        public async Task UpdateAsync(BookChapter chapter)
        {
            _booksContext.Chapters.Update(chapter);
            await _booksContext.SaveChangesAsync();
        }
    }
}
