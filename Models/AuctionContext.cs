using Microsoft.EntityFrameworkCore;

namespace auction.Models
{
    public class AuctionContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        
    }
}