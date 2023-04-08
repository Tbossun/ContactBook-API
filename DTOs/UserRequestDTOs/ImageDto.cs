using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.UserRequestDTOs
{
    public class ImageDto
    {
        public IFormFile Image { get; set; }
    }
}
