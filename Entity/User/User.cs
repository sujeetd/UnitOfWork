using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Entity
{
    public class User :  IEntity<int>
    {
        [Required(ErrorMessage = "Please specify a first name"), DataType(DataType.MultilineText)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Please specify a first name that has no more than 100 characters and no fewer than 1 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please specify a last name"), DataType(DataType.MultilineText)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Please specify a last name that has no more than 100 characters and no fewer than 1 characters")]
        public string LastName { get; set; }

        [StringLength(100, MinimumLength = 1, ErrorMessage = "Please specify a middle name that has no more than 100 characters and no fewer than 1 characters")]
        public string MiddleName { get; set; }

        public int Age { get; set; }
    }

}
