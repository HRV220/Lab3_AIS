using MyLibrary;
using System;
using System.Data.Entity;
using System.Linq;

namespace Lab3_AIS
{
    public class CarAuctionContext : DbContext
    {

        public CarAuctionContext()
            : base("CarAuctionContext")
        {

        }

        public DbSet<CarAuction> Cars { get; set; }

    }

}