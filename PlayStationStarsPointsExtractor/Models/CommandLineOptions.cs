using CommandLine;

namespace PlayStationStarsPointsExtractor.Models;

internal class CommandLineOptions
{
    [Option('i', "inputPath", Required = true, HelpText = "Set input file path.")]
    public string InputPath { get; set; } = string.Empty;

    [Option('o', "outputPath", Required = true, HelpText = "Set output file path for the generated file.")]
    public string OutputPath { get; set; } = string.Empty;
}