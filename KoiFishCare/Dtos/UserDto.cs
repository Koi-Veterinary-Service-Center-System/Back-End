using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos
{
    public class UserDto 
    {
        public string? FirstName { get; internal set; }
        public string? LastName { get; internal set; }
        public string UserName {get; set;}
        public string? Email { get; internal set; }
        public string Token {get; set;}
    }
}