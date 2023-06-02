using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace amorphie.template.core.Model;
  public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public Guid Id { get; set; }

        public Grade? Grade { get; set; }
        
        public Course Course { get; set; }
        public Student Student { get; set; }
    }