using BookStore.Models;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace BookStore.App_Start
{
    public class AzureSearch
    {
        private static ISearchServiceClient _searchClient;
        private static ISearchIndexClient _indexClient;
        private static ISearchIndexClient _studentsIndexClient;

        public static ISearchIndexClient SoccerIndexClient { get; set; }

        static AzureSearch()
        {
            string searchServiceName = "search-epam-cdp";
            string adminApiKey = "1ABA3DBC01C52CA8139D960E95B9689A";

            _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));

            IndexBooks();
            IndexSoccers();
            IndexStudents();
        }

        private static void IndexBooks()
        {
            var definition = new Index
            {
                Name = "books",
                Fields = FieldBuilder.BuildForType<Book>()
            };

            if (!_searchClient.Indexes.Exists(definition.Name))
            {
                _searchClient.Indexes.Create(definition);
            }

            if (!_searchClient.DataSources.Exists("booksdatasource"))
            {
                DataSource ds = new DataSource
                {
                    Name = "booksdatasource",
                    Type = DataSourceType.AzureSql,
                    Credentials = new DataSourceCredentials(ConfigurationManager.ConnectionStrings["BookContext"].ToString()),
                    Container = new DataContainer("Books")
                };
                _searchClient.DataSources.CreateOrUpdate(ds);
            }

            if (!_searchClient.Indexers.Exists("booksindexer"))
            {
                var nameFm = new FieldMapping("Name", FieldMappingFunction.Base64Decode());
                _searchClient.Indexers.Create(new Indexer("booksindexer", "booksdatasource", definition.Name, fieldMappings: new[] { nameFm }));
            }

            _indexClient = _searchClient.Indexes.GetClient(definition.Name);
        }

        private static void IndexStudents()
        {
            var definition = new Index
            {
                Name = "students",
                Fields = FieldBuilder.BuildForType<Student>()
            };

            if (!_searchClient.Indexes.Exists(definition.Name))
            {
                _searchClient.Indexes.Create(definition);
            }

            if (!_searchClient.DataSources.Exists("studentsdatasource"))
            {
                DataSource ds = new DataSource
                {
                    Name = "studentsdatasource",
                    Type = DataSourceType.AzureSql,
                    Credentials = new DataSourceCredentials(ConfigurationManager.ConnectionStrings["StudentsContext"].ToString()),
                    Container = new DataContainer("Students")
                };
                _searchClient.DataSources.CreateOrUpdate(ds);
            }

            if (!_searchClient.Indexers.Exists("studentsindexer"))
            {
                var fm = new FieldMapping("Id", "IndexId");
                _searchClient.Indexers.Create(new Indexer("studentsindexer", "studentsdatasource", definition.Name, fieldMappings: new[] { fm}));
            }

            _studentsIndexClient = _searchClient.Indexes.GetClient(definition.Name);
        }

        private static void IndexSoccers()
        {
            var definition = new Index
            {
                Name = "soccers",
                Fields = FieldBuilder.BuildForType<Player>()
            };

            if (!_searchClient.Indexes.Exists(definition.Name))
            {
                _searchClient.Indexes.Create(definition);
            }

            if (!_searchClient.DataSources.Exists("soccersdatasource"))
            {
                DataSource ds = new DataSource
                {
                    Name = "soccersdatasource",
                    Type = DataSourceType.AzureSql,
                    Credentials = new DataSourceCredentials(ConfigurationManager.ConnectionStrings["SoccerContex"].ToString()),
                    Container = new DataContainer("Players")
                };
                _searchClient.DataSources.CreateOrUpdate(ds);
            }

            if (!_searchClient.Indexers.Exists("soccersindexer"))
            {
                var fm = new FieldMapping("Id", "IndexId");
                _searchClient.Indexers.Create(new Indexer("soccersindexer", "soccersdatasource", definition.Name, fieldMappings: new[] { fm }));
            }

            SoccerIndexClient = _searchClient.Indexes.GetClient(definition.Name);
        }

        public IList<SearchResult<Book>> Search(string searchText)
        {
            try
            {
                SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All };
                return _indexClient.Documents.Search<Book>(searchText, sp).Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

        public IList<SearchResult<Player>> SearchSoccers(string searchText)
        {
            try
            {
                SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All };
                return SoccerIndexClient.Documents.Search<Player>(searchText, sp).Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

        public IList<SearchResult<Student>> SearchStudents(string searchText)
        {
            try
            {
                SearchParameters sp = new SearchParameters() { SearchMode = SearchMode.All };
                return _studentsIndexClient.Documents.Search<Student>(searchText, sp).Results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }
    }
}