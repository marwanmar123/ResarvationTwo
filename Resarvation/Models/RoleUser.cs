using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resarvation.Models
{
    public class RoleUser
    {

        [Display(Name = "Role")]
        public string RoleId { get; set; }
        [Display(Name = "User")]
        public string UserId { get; set; }
    }
}
