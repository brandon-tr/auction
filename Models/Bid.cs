namespace auction.Models
{
    public class Bid
    {
        public int bidId {get; set;}
        public double bid {get; set;}
        public string name {get; set;}
        public int userId {get; set;}
        public User user {get; set;}

        public int auctionId {get; set;}
        public Auction auction {get; set;}
    }
}