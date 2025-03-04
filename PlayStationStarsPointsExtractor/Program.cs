// Load the HTML content from a file
using System.Globalization;
using CommandLine;
using CsvHelper;
using HtmlAgilityPack;
using PlayStationStarsPointsExtractor.Models;

string _inputPath = string.Empty;
string _outputPath = string.Empty;

Parser.Default.ParseArguments<CommandLineOptions>(args)
    .WithParsed<CommandLineOptions>(o =>
    {
        _inputPath = o.InputPath;
        _outputPath = o.OutputPath;
    });

if (!File.Exists(_inputPath))
{
    Console.WriteLine("The input file does not exist.");
    return;
}

var htmlContent = File.ReadAllText(_inputPath);

if (string.IsNullOrWhiteSpace(htmlContent))
{
    Console.WriteLine("The HTML content is empty.");
    return;
}

// Parse the HTML content
var htmlDoc = new HtmlDocument();
htmlDoc.LoadHtml(htmlContent);

// Find all transactions
var transactions = htmlDoc.DocumentNode.SelectNodes("//li[@class='transaction']");

// Open a CSV file for writing
using (var writer = new StreamWriter(_outputPath))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteHeader<PlayStationPointTransaction>();
    csv.NextRecord();

    foreach (var transaction in transactions)
    {
        var date = transaction.SelectSingleNode(".//div[contains(@class, 'transaction__date')]").InnerText.Trim();
        var subtitle = transaction.SelectSingleNode(".//p[contains(@class, 'txt-block-title__subtitle')]").InnerText.Trim();
        var title = transaction.SelectSingleNode(".//h4[contains(@class, 'txt-block-title__title')]//span").InnerText.Trim();
        var points = transaction.SelectSingleNode(".//span[contains(@class, 'transaction__points')]").InnerText.Trim();
        var descriptionNode = transaction.SelectSingleNode(".//p[contains(@class, 'txt-block-title__paragraph')]");
        var description = descriptionNode != null ? descriptionNode.InnerText.Trim() : string.Empty;

        var expirationDate = string.Empty;
        var expirationNode = transaction.SelectNodes(".//p[contains(@class, 'txt-style-secondary') and contains(@class, 'txt-block-title__secondary') and contains(@class, 'txt-style-secondary--secondary')]")
            .FirstOrDefault(p => p.InnerText.Contains("Date d'expiration des points"));
        if (expirationNode != null)
        {
            expirationDate = expirationNode.InnerText.Split(new[] { ": " }, StringSplitOptions.None)[1].Trim();
        }

        var transactionNumber = string.Empty;
        var transactionNumberNode = transaction.SelectNodes(".//p[contains(@class, 'txt-style-secondary') and contains(@class, 'txt-block-title__secondary') and contains(@class, 'flex-space-apart')]")
            ?.FirstOrDefault(p => p.InnerText.Contains("Numéro de transaction"));
        if (transactionNumberNode != null)
        {
            transactionNumber = transactionNumberNode.SelectNodes(".//span")[1].InnerText.Trim();
        }

        var record = new PlayStationPointTransaction
        {
            Date = date,
            Subtitle = subtitle,
            Title = title,
            Points = points,
            Description = description,
            ExpirationDate = expirationDate,
            TransactionNumber = transactionNumber
        };

        csv.WriteRecord(record);
        csv.NextRecord();
    }
}

Console.WriteLine("CSV file 'transactions.csv' created successfully.");