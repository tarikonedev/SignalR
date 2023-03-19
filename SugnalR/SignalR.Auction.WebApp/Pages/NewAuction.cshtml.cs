using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using SignalR.Auction.WebApp.Hubs;
using SignalR.Auction.WebApp.Model;
using SignalR.Auction.WebApp.Repository;

namespace SignalR.Auction.WebApp.Pages
{
    public class NewAuctionModel : PageModel
    {
        private readonly IHubContext<AuctionHub> hubContext;

        public NewAuctionModel(IHubContext<AuctionHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public void OnGet()
        {
        }

        public void OnPost(string name) 
        {
            var context = new AuctionContext();

            var maxid = context.Lots.Select(p => p.Id).Max();

            var newLot = new Lot { Id = maxid + 1, Name = name, Bid = 0 };

            context.Lots.Add(newLot);
            context.SaveChanges();

            hubContext.Clients.All.SendAsync("ReceiveNewLot", newLot);
        }
    }
}
