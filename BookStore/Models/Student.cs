using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public Student()
        {
            Courses = new List<Course>();
        }
    }
}