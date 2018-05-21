using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BookStore.Models.Pagination;
using Swashbuckle.Swagger.Annotations;
using System.Configuration;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;

namespace BookStore.Controllers
{
    /// <summary>
    /// Pagination API for navigation among phones list
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class PaginationApiController : ApiController
    {
        private DocumentClient cosmosClient;
        private Uri dbUri;
        private List<Phone> phones;

        public PaginationApiController()
        {
            cosmosClient = new DocumentClient(new Uri(ConfigurationManager.AppSettings["CosmosEndpoint"]), ConfigurationManager.AppSettings["CosmosKey"]);
            dbUri = UriFactory.CreateDocumentCollectionUri("AzurePhoneStorage", "Phones");
        }
        /*
        private List<Phone> phones = new List<Phone>
        {

            new Phone {Id = 1, Model = "Samsung Galaxy III", Producer = "Samsung"},
            new Phone {Id = 2, Model = "Samsung Ace II", Producer = "Samsung"},
            new Phone {Id = 3, Model = "HTC Hero", Producer = "HTC"},
            new Phone {Id = 4, Model = "HTC One S", Producer = "HTC"},
            new Phone {Id = 5, Model = "HTC One X", Producer = "HTC"},
            new Phone {Id = 6, Model = "LG Optimus 3D", Producer = "LG"},
            new Phone {Id = 7, Model = "Nokia N9", Producer = "Nokia"},
            new Phone {Id = 8, Model = "Samsung Galaxy Nexus", Producer = "Samsung"},
            new Phone {Id = 9, Model = "Sony Xperia X10", Producer = "SONY"},
            new Phone {Id = 10, Model = "Samsung Galaxy II", Producer = "Samsung"}
        };
        */
        /// <summary>
        /// Test request
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        [SwaggerResponse(500, "Bad data")]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, "Returns the fake data", typeof(PaginationViewModel))]
        public async Task<PaginationViewModel> Get(int page)
        {
            int pageSize = 3;
            IEnumerable<Phone> phonesPerPages = new List<Phone>();

            try
            {
                var feedOptions = new FeedOptions
                {
                    MaxItemCount = 3
                };

                var query = cosmosClient.CreateDocumentQuery<Phone>(dbUri, feedOptions).AsDocumentQuery();
                var currentPage = 0;

                while (query.HasMoreResults && currentPage++ < page)
                {
                    phonesPerPages = await query.ExecuteNextAsync<Phone>();
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return null;
            }

            //var phonesPerPages = phones.Skip((page - 1) * pageSize).Take(pageSize);
            var pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = 10
            };

            return new PaginationViewModel
            {
                PageInfo = pageInfo,
                Phones = phonesPerPages.ToList()
            };
        }

        public async Task Post([FromBody]Phone value)
        {
            if (!ModelState.IsValid)
            {
                return ;
            }
            var maxPhoneId = cosmosClient.CreateDocumentQuery<Phone>(dbUri).Max<Phone>(x => x.Id);
            value.Id = ++maxPhoneId;
            try
            {
                await cosmosClient.CreateDocumentAsync(dbUri, value);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}