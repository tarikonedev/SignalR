using Microsoft.AspNetCore.SignalR;

namespace SignalR.Auction.WebApp.Hubs
{
    public class AuctionHub : Hub
    {
        public async Task NotifyNewBid(AuctionNotify auctionNotify) 
        {
            await Clients.All.SendAsync("ReceiveNewBid", auctionNotify);   
        }
    }
}
