using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HotelListingSystem.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //public class ApplicationUser : IdentityUser
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SystemIdentityUser> manager)
    //    {
    //        // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //        var userIdentity = await manager.CreateIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
    //        // Add custom user claims here
    //        return userIdentity;
    //    }
    //}

    public class ApplicationDbContext : IdentityDbContext<SystemUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<HotelListingSystem.Models.Dining> Dinings { get; set; }

        public System.Data.Entity.DbSet<HotelListingSystem.Models.DiningTableTypes> DiningTableTypes { get; set; }

        public System.Data.Entity.DbSet<HotelListingSystem.Models.MealTypes> MealTypes { get; set; }
        public System.Data.Entity.DbSet<HotelListingSystem.Models.DiningReservation> DiningReservations { get; set; }
        public System.Data.Entity.DbSet<HotelListingSystem.Models.Payment> Payments { get; set; }
        public System.Data.Entity.DbSet<HotelListingSystem.Models.Hotel> Hotels { get; set; }
        public System.Data.Entity.DbSet<HotelListingSystem.Models.HotelUsers> HotelUsers { get; set; }
        public System.Data.Entity.DbSet<HotelListingSystem.Models.Business> Businesses { get; set; }
        public System.Data.Entity.DbSet<HotelListingSystem.Models.Room> Rooms { get; set; }

        public System.Data.Entity.DbSet<HotelListingSystem.Models.Reservation> Reservations { get; set; }
    }
}