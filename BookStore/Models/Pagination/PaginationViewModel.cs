using System.Collections.Generic;

namespace BookStore.Models.Pagination
{
    /// <summary>
    /// Returns the fake data
    /// </summary>
    public class PaginationViewModel
    {
        public IEnumerable<Phone> Phones { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}