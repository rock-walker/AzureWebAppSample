using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Player
    {
        [JsonIgnore]
        [JsonConverter(typeof(StringConverter))]
        [Key]
        public Guid Id { get; set; }

        [IsRetrievable(false)]
        [NotMapped]
        public string IndexId {
            get { return Id.ToString(); }
            set { value = Id.ToString(); }
        }

        [IsSearchable]
        public string Name { get; set; }

        [IsFilterable]
        public int Age { get; set; }

        [IsSearchable]
        public string Position { get; set; }

        public int? TeamId { get; set; }

        [JsonIgnore]
        public Team Team { get; set; }
    }

    public class StringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Guid id = (Guid)value;

            writer.WriteValue(id.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = (string)reader.Value;

            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid);
        }
    }

}