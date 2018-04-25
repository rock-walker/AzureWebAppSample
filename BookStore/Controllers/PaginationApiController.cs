using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BookStore.Models.Pagination;

namespace BookStore.Controllers
{
    public class PaginationApiController : ApiController
    {
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

        // GET api/<controller>/5
        public PaginationViewModel Get(int page)
        {
            int pageSize = 3;
            var phonesPerPages = phones.Skip((page - 1) * pageSize).Take(pageSize);
            var pageInfo = new PageInfo
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = phones.Count
            };

            return new PaginationViewModel
            {
                PageInfo = pageInfo,
                Phones = phonesPerPages
            };
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}