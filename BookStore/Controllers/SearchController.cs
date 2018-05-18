using BookStore.App_Start;
using BookStore.Models;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class SearchController : Controller
    {
        private AzureSearch azureSearch = new AzureSearch();
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

            var books = azureSearch.Search(query);

            var soccers = azureSearch.SearchSoccers(query);

            var students = azureSearch.SearchStudents(query);

            var output = Tuple.Create(books, soccers, students);

            return View(output);
        }
    }
}