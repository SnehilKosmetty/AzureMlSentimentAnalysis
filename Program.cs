using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;
using System;
using static System.Net.WebRequestMethods;

class Program
{
    static void Main()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddUserSecrets<Program>()       // <-- IMPORTANT
            .AddEnvironmentVariables()
            .Build();



        string endpoint = config["AzureAI:Endpoint"];
        string apiKey = config["AzureAI:Key"];

        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("ERROR: Missing Endpoint or Key. Add them using User Secrets.");
            return;
        }

        var credentials = new AzureKeyCredential(apiKey);
        var client = new TextAnalyticsClient(new Uri(endpoint), credentials);

        // Text to analyze
        string inputText = "I really love using Azure, but sometimes the documentation is confusing.";

        // Call sentiment analysis
        DocumentSentiment documentSentiment = client.AnalyzeSentiment(inputText);

        Console.WriteLine($"Text: {inputText}");


        Console.WriteLine($"Overall sentiment: {documentSentiment.Sentiment}");
        Console.WriteLine();

        Console.WriteLine("Sentence-level details:");
        foreach (var sentence in documentSentiment.Sentences)
        {
            Console.WriteLine($"  Sentence: \"{sentence.Text}\"");
            Console.WriteLine($"  Sentiment: {sentence.Sentiment}");
            Console.WriteLine($"  Positive score: {sentence.ConfidenceScores.Positive:0.00}");
            Console.WriteLine($"  Neutral score: {sentence.ConfidenceScores.Neutral:0.00}");
            Console.WriteLine($"  Negative score: {sentence.ConfidenceScores.Negative:0.00}");
            Console.WriteLine();
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
