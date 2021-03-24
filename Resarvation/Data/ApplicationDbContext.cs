using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Resarvation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resarvation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Apprenant> Apprenants { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<TypeReservation> TypeReservations { get; set; }
        //public DbSet<Resarvation.Models.ReservApprenantViewModel> ReservApprenantViewModel { get; set; }
        //public DbSet<Resarvation.Models.ReservApprenantViewModel> ReservApprenantViewModel { get; set; }


    }
}
