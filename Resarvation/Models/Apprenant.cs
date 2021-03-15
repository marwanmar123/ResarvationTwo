using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resarvation.Models
{
    public class Apprenant : IdentityUser
    {
        public string Class { get; set; }
        public int ResCount { get; set; }


    }
}
