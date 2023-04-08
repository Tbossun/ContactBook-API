using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UserRequestDTOs
{
    public class UpdateRequestDTO
    {
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
       
        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        
        public string StreetAddress { get; set; }

        [DataType(DataType.Text)]
        public string City { get; set; }

       
        [DataType(DataType.Text)]
        public string State { get; set; }

       
       // ErrorMessage = "UserName should contain only Upper case, lower case, numbers and underscore characters")]
        public string UserName { get; set; }
    }
}
