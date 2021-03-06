using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Resarvation.Models
{
    public class ReservApprenantViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public bool? Status { get; set; }
        public string Cause { get; set; }
        public string TypeReservationId { get; set; }
        public string Name { get; set; }
        public Reservation Reservation { get; set; }
        public TypeReservation typeReservation { get; set; }
        public int ResCount { get; set; }

        public List<string> Roles { get; set; }
    }
}
