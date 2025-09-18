using ConfigCat.Client;

Console.WriteLine("test");

var client = ConfigCatClient.Get("configcat-sdk-1/D0vdCEFN30uPhcE4NpqzEQ/WUffFKazcEOC1-ZXIHmSfg"); // <-- This is the actual SDK Key for your 'Test Environment' environment
client.LogLevel = LogLevel.Info; // <-- Set the log level to INFO to track how your feature flags were evaluated. When moving to production, you can remove this line to avoid too detailed logging.

Console.WriteLine();

// Get Feature Flags and settings from dashboard
var isMyFirstFeatureEnabled = await client.GetValueAsync("isMyFirstFeatureEnabled", false);
var dotnetconfigcat1feat = await client.GetValueAsync("dotnetconfigcat1feat", false);

Console.WriteLine("isMyFirstFeatureEnabled's value from ConfigCat: " + isMyFirstFeatureEnabled);
Console.WriteLine("dotnetconfigcat1feat's value from ConfigCat: " + dotnetconfigcat1feat);
