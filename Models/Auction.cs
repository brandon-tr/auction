using System;
using System.Collections.Generic;

namespace auction.Models
{
    public class Auction
    {
        public int auctionId { get; set; }

        public double currentBid {get; set;}
        public int creatorId { get; set; }
        public string product { get; set; }

        public string description { get; set; }

        public DateTime createdAt{get; set;}

        public DateTime endDate{get; set;}

        public List<Bid> bids { get; set; }
        public Auction()
        {   
            bids = new List<Bid>();
        }
    }
}