using BookStore.App_Start;
using BookStore.Models;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        private AzureSearch azureSearch = new AzureSearch();
        private ConnectionMultiplexer redisConnection = RedisConfig.RedisConnection;
        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                query = "*";
            }

            var books = GetCachedBooks(query);

            var soccers = GetCachedSoccers(query);

            var students = GetCachedStudents(query);

            var output = Tuple.Create(books, soccers, students);

            return View(output);
        }

        private IList<SearchResult<Book>> GetCachedBooks(string query)
        {
            IList<SearchResult<Book>> books;

            IDatabase cache = redisConnection.GetDatabase();
            RedisValue serializedBooks = cache.StringGet("books|" + query);
            if (!serializedBooks.IsNullOrEmpty)
            {
                books = JsonConvert.DeserializeObject<IList<SearchResult<Book>>>(serializedBooks);
            }
            else
            {
                books = azureSearch.Search(query);
                cache.StringSetAsync("books|" + query, JsonConvert.SerializeObject(books), TimeSpan.FromSeconds(30));
            }

            return books;
        }

        private IList<SearchResult<Player>> GetCachedSoccers(string query)
        {
            IList<SearchResult<Player>> books = null;

            IDatabase cache = redisConnection.GetDatabase();
            var serializedBooks = cache.StringGet(query + ":soccers");
            if (!serializedBooks.IsNullOrEmpty)
            {
                books = JsonConvert.DeserializeObject<IList<SearchResult<Player>>>(serializedBooks);
            }
            else
            {
                books = azureSearch.SearchSoccers(query);
                cache.StringSet(query + ":soccers", JsonConvert.SerializeObject(books), TimeSpan.FromSeconds(30));
            }

            return books;
        }

        private IList<SearchResult<Student>> GetCachedStudents(string query)
        {
            IList<SearchResult<Student>> books = null;

            IDatabase cache = redisConnection.GetDatabase();
            var serializedBooks = cache.StringGet(query + ":students");
            if (!serializedBooks.IsNullOrEmpty)
            {
                books = JsonConvert.DeserializeObject<IList<SearchResult<Student>>>(serializedBooks);
            }
            else
            {
                books = azureSearch.SearchStudents(query);
                cache.StringSet(query + ":students", JsonConvert.SerializeObject(books), TimeSpan.FromSeconds(30));
            }

            return books;
        }
    }
}