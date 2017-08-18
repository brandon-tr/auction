using System;
using System.ComponentModel.DataAnnotations;

namespace auction.Models
{
    public class AuctionReg
    {
        [Required]
        [MinLength(3)]
        public string product {get; set;}

        [Required]
        [MinLength(10)]
        public string description {get; set;}

        [Required]
        public double bid {get; set;}

        [Required]
        public DateTime endDate{get; set;}

        [Required]
        public double currentBid {get; set;}
    }
}