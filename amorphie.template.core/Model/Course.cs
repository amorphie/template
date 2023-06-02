using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amorphie.template.core.Model;
  public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        
        public ICollection<Enrollment> Enrollments { get; set; }
    }