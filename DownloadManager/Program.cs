using DownloadManager;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddWindowsService();
        services.AddHostedService<DownloadService>();

    })
    .UseSerilog()
    .Build();

var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(configurations["Logging:Logpath"]!)
    .CreateLogger();

host.Run();
