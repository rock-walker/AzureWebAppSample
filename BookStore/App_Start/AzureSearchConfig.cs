using BookStore.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;

namespace BookStore.App_Start
{
    public class AzureSearch
    {
        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;

        static AzureSearch()
        {
            string searchServiceName = "search-epam-cdp";
            string adminApiKey = "1ABA3DBC01C52CA8139D960E95B9689A";

            _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            var definition = new Index
            {
                Name = "books",
                Fields = FieldBuilder.BuildForType<Book>()
            };

            if (!_searchClient.Indexes.Exists(definition.Name))
            {
                _searchClient.Indexes.Create(definition);
            }

            _indexClient = _searchClient.Indexes.GetClient(definition.Name);
        }

        public DocumentSearchResult Search(string searchText)
        {
            try
            {
                SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All };
                return _indexClient.Documents.Search(searchText, sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }
    }
}