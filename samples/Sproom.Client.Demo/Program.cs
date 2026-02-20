using System;
using System.Linq;
using System.Threading.Tasks;
using Sproom.Client;
using Sproom.Client.Models;

namespace Sproom.Client.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var token = Environment.GetEnvironmentVariable("SPROOM_API_TOKEN");
            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Set the SPROOM_API_TOKEN environment variable");
                return;
            }

            var options = new SproomClientOptions
            {
                BaseUrl = "https://staging.sproom.net",
                ApiToken = token
            };

            using (var client = new SproomClient(options))
            {
                // Health check
                Console.WriteLine("Checking API health...");
                var healthy = await client.IsHealthyAsync();
                Console.WriteLine($"API healthy: {healthy}");

                if (!healthy)
                {
                    Console.WriteLine("API is not available. Exiting.");
                    return;
                }

                // List documents
                Console.WriteLine("\nFetching documents...");
                try
                {
                    var documents = await client.GetDocumentsAsync();
                    Console.WriteLine($"Found {documents.Count} documents");
                    foreach (var doc in documents.Take(5))
                    {
                        Console.WriteLine($"  - {doc.DocumentId}: {doc.Type} | {doc.Status} | {doc.TotalAmount} {doc.Currency}");
                    }
                }
                catch (SproomApiException ex)
                {
                    Console.WriteLine($"Error fetching documents: [{ex.StatusCode}] {ex.Message}");
                }

                // List webhooks
                Console.WriteLine("\nFetching webhooks...");
                try
                {
                    var webhooks = await client.GetWebhooksAsync();
                    Console.WriteLine($"Found {webhooks.Count} webhooks");
                    foreach (var wh in webhooks)
                    {
                        Console.WriteLine($"  - {wh.Id}: {wh.Type} -> {wh.Url}");
                    }
                }
                catch (SproomApiException ex)
                {
                    Console.WriteLine($"Error fetching webhooks: [{ex.StatusCode}] {ex.Message}");
                }

                // List registrations
                Console.WriteLine("\nFetching registrations...");
                try
                {
                    var registrations = await client.GetRegistrationsAsync();
                    Console.WriteLine($"Found {registrations.Count} registrations");
                    foreach (var reg in registrations)
                    {
                        Console.WriteLine($"  - {reg.NetworkId}: {reg.OrganizationIdentifier.SchemeId}:{reg.OrganizationIdentifier.Value}");
                    }
                }
                catch (SproomApiException ex)
                {
                    Console.WriteLine($"Error fetching registrations: [{ex.StatusCode}] {ex.Message}");
                }

                // List subscriptions
                Console.WriteLine("\nFetching subscriptions...");
                try
                {
                    var subscriptions = await client.GetSubscriptionsAsync();
                    Console.WriteLine($"Found {subscriptions.Count} subscriptions");
                    foreach (var sub in subscriptions)
                    {
                        Console.WriteLine($"  - {sub.SubscriptionId}: {sub.ServiceType} (since {sub.SubscribedOnUtc:yyyy-MM-dd})");
                    }
                }
                catch (SproomApiException ex)
                {
                    Console.WriteLine($"Error fetching subscriptions: [{ex.StatusCode}] {ex.Message}");
                }

                Console.WriteLine("\nDone!");
            }
        }
    }
}
