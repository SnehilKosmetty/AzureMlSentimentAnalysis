using System;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;

public class AzureTextAnalyzer
{
    private readonly TextAnalyticsClient _client;

    public AzureTextAnalyzer(TextAnalyticsClient client)
    {
        _client = client;
    }

    public async Task AnalyzeAllAsync(string text)
    {
        Console.WriteLine();
        Console.WriteLine("Analyzing text with Azure AI Language...");
        Console.WriteLine($"Text: {text}");
        Console.WriteLine(new string('-', 60));

        await DetectLanguageAsync(text);
        await AnalyzeSentimentAsync(text);
        await ExtractKeyPhrasesAsync(text);
    }

    private async Task DetectLanguageAsync(string text)
    {
        Console.WriteLine(">> Language Detection");

        DetectedLanguage language = await _client.DetectLanguageAsync(text);
        Console.WriteLine($"  Language: {language.Name} (ISO: {language.Iso6391Name})");
        Console.WriteLine($"  Confidence: {language.ConfidenceScore:0.00}");
        Console.WriteLine();
    }

    private async Task AnalyzeSentimentAsync(string text)
    {
        Console.WriteLine(">> Sentiment Analysis");

        DocumentSentiment sentiment = await _client.AnalyzeSentimentAsync(text);

        Console.WriteLine($"  Overall sentiment: {sentiment.Sentiment}");
        Console.WriteLine($"  Scores -> Pos: {sentiment.ConfidenceScores.Positive:0.00}, " +
                          $"Neu: {sentiment.ConfidenceScores.Neutral:0.00}, " +
                          $"Neg: {sentiment.ConfidenceScores.Negative:0.00}");

        Console.WriteLine("  Sentence-level:");
        foreach (var sentence in sentiment.Sentences)
        {
            Console.WriteLine($"    Sentence: {sentence.Text}");
            Console.WriteLine($"    Sentiment: {sentence.Sentiment}");
            Console.WriteLine($"    Pos: {sentence.ConfidenceScores.Positive:0.00}, " +
                              $"Neu: {sentence.ConfidenceScores.Neutral:0.00}, " +
                              $"Neg: {sentence.ConfidenceScores.Negative:0.00}");
            Console.WriteLine();
        }
    }

    private async Task ExtractKeyPhrasesAsync(string text)
    {
        Console.WriteLine(">> Key Phrase Extraction");

        var response = await _client.ExtractKeyPhrasesAsync(text);

        Console.WriteLine("  Key phrases:");
        foreach (string phrase in response.Value)
        {
            Console.WriteLine($"    - {phrase}");
        }

        Console.WriteLine();
    }
}
