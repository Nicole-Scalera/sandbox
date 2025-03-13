using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigCat.Client;

// SDK to connect to ConfigCat dashboard
var client = ConfigCatClient.Get("configcat-sdk-1/D0vdCEFN30uPhcE4NpqzEQ/WUffFKazcEOC1-ZXIHmSfg");

// Sample code from dashboard
var isMyFirstFeatureEnabled = await client.GetValueAsync("isMyFirstFeatureEnabled", false);
var dotnetconfigcat1feat = await client.GetValueAsync("dotnetconfigcat1feat", false);

// Write to console if flag is on/off (bool)
Console.WriteLine("isMyFirstFeatureEnabled's value from ConfigCat: " + isMyFirstFeatureEnabled);
Console.WriteLine("dotnetconfigcat1feat's value from ConfigCat: " + dotnetconfigcat1feat);