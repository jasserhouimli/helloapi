using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class UserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}