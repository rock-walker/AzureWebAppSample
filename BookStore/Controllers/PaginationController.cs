using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Models.Pagination;

namespace BookStore.Controllers
{
    public class PaginationController : Controller
    {
      List<Phone> phones;
      public PaginationController()
        {
	        phones = new List<Phone>
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
        }
        // GET: Pagination
      public ActionResult Index(int page = 1)
        {
            int pageSize = 3; // количество объектов на страницу
            IEnumerable<Phone> phonesPerPages = phones.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = phones.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Phones = phonesPerPages };
            return View(ivm);
        }
    }
}