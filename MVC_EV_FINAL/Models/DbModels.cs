using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC_EV_FINAL.Models
{
	public class Client
	{
		public Client()
		{
			this.bookingEntries = new List<BookingEntry>();
		}
		public int ClientId { get; set; }
		[Required, Display(Name = "Name"), StringLength(80)]
		public string ClientName { get; set; }
		[Required, Display(Name = "Date of Birth"), DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime BirthDate { get; set; }
		public int Age { get; set; }
		public string Picture { get; set; }
		public bool MaritalStatus { get; set; }

        public ICollection<BookingEntry> bookingEntries { get; set; }

    }
	public class Spot
	{
		public Spot()
		{
            this.bookingEntries = new List<BookingEntry>();
        }
		public int SpotId { get; set; }
        [Required, Display(Name = "Name"), StringLength(80)]
        public string SpotName { get; set; }
		public ICollection<BookingEntry> bookingEntries { get; set; }
	}
	public class BookingEntry
	{
		public int BookingEntryId { get; set; }
		[ForeignKey("Client")]
		public int ClientId { get; set; }
        [ForeignKey("Spot")]
        public int SpotId { get; set; }

		//Nav
		public virtual Client Client { get; set; }
		public virtual Spot Spot { get; set; }
	}
	public class BookingDbContext : DbContext
	{
		public DbSet<Client> Clients { get; set; }
		public DbSet<Spot> Spots{ get; set; }
		public  DbSet<BookingEntry> BookingEntries { get; set; }
	}
}