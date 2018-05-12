using BookStore.App_Start;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

            DocumentSearchResult results = azureSearch.Search(query);

            return View(results.Results);
        }
    }
}