using System.Collections.Generic;
using System;

namespace auction.Models
{
    public class User
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public double money {get; set;}

        public List<Bid> bids { get; set; }
        public User()
        {
            bids = new List<Bid>();
        }
    }
}