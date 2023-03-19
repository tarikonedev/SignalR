using SignalR.Auction.WebApp.Hubs;
using SignalR.Auction.WebApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSignalR();
    //.AddAzureSignalR("connection_string")
    //.AddStackExchangeRedis("redis_connection_string");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapMethods("auction/{auctionId}/newid", new[] { "POST", "PUT" }, (int auctionId, double currentBid) =>
{
    using var auctionContext = new AuctionContext();

    var lot = auctionContext.Lots.Find(auctionId);

    lot.Bid = currentBid;

    auctionContext.Lots.Update(lot);

    auctionContext.SaveChanges();

    return Results.Ok();
});

app.MapGet("auctions", () =>
{
    using var auctionContext = new AuctionContext();

    var lots = auctionContext.Lots.ToList();

    return Results.Ok(lots);
});

app.MapHub<AuctionHub>("/auctionhub");

app.Run();
