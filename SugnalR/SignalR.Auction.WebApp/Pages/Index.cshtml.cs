using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SignalR.Auction.WebApp.Model;
using SignalR.Auction.WebApp.Repository;

namespace SignalR.Auction.WebApp.Pages
{
    public class IndexModel : PageModel
    {

        private readonly ILogger<IndexModel> _logger;

        public string PageName { get; set; }

        public List<Lot> Lots = new List<Lot>();


        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            PageName = "My first App";

            using var auctionContext = new AuctionContext();

            var lots = auctionContext.Lots.ToList();

            Lots = lots;
        }

        public void OnPost(int id, double sum) 
        {
            using var auctionContext = new AuctionContext();

            var lot = auctionContext.Lots.Find(id);

            lot.Bid = sum;

            auctionContext.Lots.Update(lot);

            auctionContext.SaveChanges();

            Lots = auctionContext.Lots.ToList();
        }
    }
}