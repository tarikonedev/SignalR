// See https://aka.ms/new-console-template for more information
using ConsoleClient;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

Console.WriteLine("Hello, World!");

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://localhost:7050");
var response = await httpClient.GetAsync("/auctions");
var auctions = await response.Content.ReadFromJsonAsync<List<Lot>>();

foreach (var auction in auctions) 
{
    Console.WriteLine($"{auction.Id, -3} {auction.Name,-20} " + $"{auction.Bid, 10}");
}

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7050/auctionhub")
    .Build();

connection.On("ReceiveNewBid", (AuctionNotify auctionNotify) =>
{
    var auction = auctions
        .Single(a => a.Id == auctionNotify.LotId);

    auction.Bid = auctionNotify.Bid;

    Console.WriteLine("New bid:");
    Console.WriteLine($"{auction.Id,-3} {auction.Name,-20} " + $"{auction.Bid,10}");
});

connection.On("ReceiveNewLot", (Lot lot) =>
{
    auctions.Add(lot);

    var auction = lot;

    Console.WriteLine("New lot:");
    Console.WriteLine($"{auction.Id,-3} {auction.Name,-20} " + $"{auction.Bid,10}");
});

try
{
    await connection.StartAsync();
}
catch (Exception ex) 
{
    Console.WriteLine(ex.Message);
}

try
{
    while (true) 
    {
        Console.WriteLine("Auction id?");
        var id = Console.ReadLine();

        Console.WriteLine($"New bid for auction {id}?");
        var bid = Console.ReadLine();
        await connection.InvokeAsync("NotifyNewBid", new { LotId = int.Parse(id!), Bid = int.Parse(bid!) });
        Console.WriteLine("Bid placed");
    }
}
finally 
{
    await connection.StopAsync();
}

