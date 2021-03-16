using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Resarvation.Models
{
    public class Reservation
    {
        //public Reservation()
        //{
        //    Status = Models.Status.Waiting.ToString();
        //}
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public bool? Status { get; set; }
        public string Cause { get; set; }

        public string ApprenantId { get; set; }
        public Apprenant Apprenant { get; set; }

        public string TypeReservationId { get; set; }
        public TypeReservation TypeReservation { get; set; }
    }
}
