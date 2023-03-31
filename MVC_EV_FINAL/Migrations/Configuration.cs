namespace MVC_EV_FINAL.Migrations
{
    using MVC_EV_FINAL.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MVC_EV_FINAL.Models.BookingDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        public List<Spot> spotList = new List<Spot>();

        protected override void Seed(MVC_EV_FINAL.Models.BookingDbContext context)
        {
            spotList.Add(new Spot() { SpotName = "Dhaka" });
            spotList.Add(new Spot() { SpotName = "Chittagong" });
            spotList.Add(new Spot() { SpotName = "Barishal" });
            spotList.Add(new Spot() { SpotName = "Rajshahi" });
            spotList.Add(new Spot() { SpotName = "Khulna" });

            context.Spots.AddRange(spotList);
        }
    }
}
