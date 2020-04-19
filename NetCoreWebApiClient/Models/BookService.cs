using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetCoreWebApiClient.Models
{
    public class BookService: IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;
        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<BookChapter>> GetBookChapters()
        {
            var uri = "https://localhost:44310/api/BookChapters";// API.Catalog.GetAllCatalogItems(_remoteServiceBaseUrl, page, take, brand, type);

            var responseString = await _httpClient.GetStringAsync(uri);

            var catalog = JsonConvert.DeserializeObject<IEnumerable<BookChapter>>(responseString);
            return catalog;
        }
    }
}
