using Microsoft.Azure.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Models
{
    public class Student
    {
        [JsonIgnore]
        [JsonConverter(typeof(IntToStringConverter))]
        [Key]
        public int Id { get; set; }

        [IsRetrievable(true)]
        [NotMapped]
        public string IndexId
        {
            get
            {
                return Id.ToString();
            }
            set
            {
                value = Id.ToString();
            }
        }

        [IsSearchable]
        public string Name { get; set; }
        [IsSearchable]
        public string Surname { get; set; }

        [JsonIgnore]
        public virtual ICollection<Course> Courses { get; set; }
        public Student()
        {
            Courses = new List<Course>();
        }
    }

    public class IntToStringConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return long.Parse(reader.Value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}