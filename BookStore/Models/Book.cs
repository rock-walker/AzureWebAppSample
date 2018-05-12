using Microsoft.Azure.Search;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Key]
        [IsSearchable]
        public string Name { get; set; }
        [IsSearchable]
        public string Author { get; set; }
        [IsFacetable, IsFilterable]
        public int? Price { get; set; }
    }
}