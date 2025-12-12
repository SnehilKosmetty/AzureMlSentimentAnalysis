using System;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;

class Program
{
    static async Task Main()
    {
        // Load configuration
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<Program>()
            .AddEnvironmentVariables()
            .Build();

        string endpoint = config["AzureAI:Endpoint"];
        string apiKey = config["AzureAI:Key"];

        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("ERROR: Missing AzureAI:Endpoint or AzureAI:Key in configuration.");
            return;
        }

        var credentials = new AzureKeyCredential(apiKey);
        var client = new TextAnalyticsClient(new Uri(endpoint), credentials);

        var analyzer = new AzureTextAnalyzer(client);

        Console.WriteLine("=== Azure AI Language Demo ===");
        Console.WriteLine("Type a sentence (or just press Enter to exit).");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Input text: ");
            string? text = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            try
            {
                await analyzer.AnalyzeAllAsync(text);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while calling Azure AI Language:");
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine();
        }
    }
}
