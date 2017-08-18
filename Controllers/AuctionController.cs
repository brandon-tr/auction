using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using auction.Models;
using Microsoft.EntityFrameworkCore;

namespace auction.Controllers
{
    public class AuctionController : Controller
    {
        private AuctionContext _context;
        public AuctionController(AuctionContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("/");
            }
            User user = _context.users.SingleOrDefault(x => x.userId == HttpContext.Session.GetInt32("id"));
            ViewBag.Wallet = user.money;
            var auction = _context.auctions.Include(x => x.bids).ThenInclude(c => c.user).ToList();
            ViewBag.AllAuctions = auction;
            foreach (var item in auction)
            {
                if (item.endDate == DateTime.Now)
                {
                    Auction removeAuction = _context.auctions.Where(x => x.endDate == DateTime.Now).Include(c => c.bids).ThenInclude(i => i.user).SingleOrDefault();
                    if (removeAuction.bids[0].user.userId == removeAuction.creatorId)
                    {
                        if(removeAuction.bids[0].userId == removeAuction.bids[0].user.userId){
                            removeAuction.bids[0].user.money += removeAuction.bids[0].bid;
                            _context.SaveChanges();
                        }
                        _context.Remove(removeAuction);
                        _context.SaveChanges();
                    }
                }
            }
            ViewBag.Id = HttpContext.Session.GetInt32("id");
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        [HttpGet]
        [Route("Auction/Delete/{auctionId}")]
        public IActionResult Delete(int auctionId)
        {
            Auction getAuction = _context.auctions.Where(x => x.auctionId == auctionId).SingleOrDefault();
            _context.auctions.Remove(getAuction);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Auction")]
        public IActionResult Auction()
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("/");
            }
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [HttpPost]
        [Route("AddAuction")]
        public IActionResult AddAuction(AuctionReg newAuction)
        {
            if (ModelState.IsValid)
            {
                if (newAuction.endDate < DateTime.Now)
                {
                    TempData["Error"] = "End Date must be in the future";
                    return RedirectToAction("Auction");
                }
                else if (newAuction.currentBid == 0)
                {
                    TempData["Error"] = "Starting Bid must be greater than 0";
                    return RedirectToAction("Auction");
                }
                else
                {
                    Auction auction = new Auction()
                    {
                        product = newAuction.product,
                        creatorId = (int)HttpContext.Session.GetInt32("id"),
                        description = newAuction.description,
                        currentBid = newAuction.currentBid,
                        createdAt = DateTime.Now,
                        endDate = newAuction.endDate,
                    };
                    _context.auctions.Add(auction);
                    _context.SaveChanges();
                    Bid bid = new Bid()
                    {
                        userId = (int)HttpContext.Session.GetInt32("id"),
                        auctionId = auction.auctionId,
                        bid = newAuction.bid,
                        name = "No One",
                    };
                    _context.bids.Add(bid);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
            }
            return View("Auction");
        }
        [HttpGet]
        [Route("Auction/{auctionId}")]
        public IActionResult Bid(int auctionId)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("/");
            }
            var auction = _context.auctions.Where(x => x.auctionId == auctionId).Include(x => x.bids).ThenInclude(c => c.user).SingleOrDefault();
            ViewBag.AllAuctions = auction;
            ViewBag.Id = HttpContext.Session.GetInt32("id");
            return View();
        }

        [HttpPost]
        [Route("Auction/PlaceBid/{auctionId}")]
        public IActionResult PlaceBid(int auctionId, double bid)
        {
            if (HttpContext.Session.GetInt32("id") == null)
            {
                return Redirect("/");
            }
            var bids = _context.bids.Where(x => x.auctionId == auctionId).SingleOrDefault();
            var auction = _context.auctions.Where(x => x.auctionId == auctionId).SingleOrDefault();
            var subtractMoney = _context.users.Where(x => x.userId == HttpContext.Session.GetInt32("id")).SingleOrDefault();
            if (bids.bid >= bid || subtractMoney.money < bid || auction.currentBid > bid)
            {
                return RedirectToAction("Bid", auctionId);

            }
            else
            {
                bids.bid = bid;
                _context.SaveChanges();
                bids.name = HttpContext.Session.GetString("name");
                _context.SaveChanges();
                subtractMoney.money -= bid;
                _context.SaveChanges();
            }
            return RedirectToAction("Bid", auctionId);
        }
    }
}
