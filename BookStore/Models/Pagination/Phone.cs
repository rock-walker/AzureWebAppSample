using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models.Pagination
{
    public class Phone
    {
        [JsonProperty(PropertyName="id")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int Id { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Producer { get; set; }
    }

   
}