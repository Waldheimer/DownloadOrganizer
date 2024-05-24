# Project Setup
- [x] Create a new **Worker-Service**-Project
- [x] Install Nuget-Package for WindowsService
    - [x] Microsoft.Extensions.Hosting.WindowsServices   
- [x] Install Nuget-Packages for Logging
	- [x] Serilog.AspNetCore
	- [x] Serilog.Sinks.File
    - [x] Add the Logpath to the **appsettings.json** File under Logging
    ```json
    "Logging": {
        "Logpath": "J:\\Logging\\DownloadService\\DownloadServiceLogs.log",
        "LogLevel": {
          "Default": "Information",
          "Microsoft.Hosting.Lifetime": "Information"
        }
      }
    ```

    - [x] between `.Build();` and `host.Run();` insert the Logger-Configuration

    ```cs
    var configurations = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
        .Enrich.FromLogContext()
        .WriteTo.File(configurations["Logging:Logpath"]!)
        .CreateLogger();
    ```
- [x] Reconfigure the HostBuilder in **Program.cs**
```cs
using DownloadManager;
using Serilog;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddWindowsService();
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

host.Run();
```

# New Service
- [x] Create a new Class-File
	- [x] overrode the **StartAsync**-Method
	- [x] override the **StopAsync**-Method

- [x] Include the Class in DI with
```cs
builder.Services.AddHostedService<NewWOrkerClass>();
```

# Publish the Service
- [x] Rightclick the Project and select **Publish**
- [x] Select the Output-Option and publish the Service

# Install, Start, Stop & UnInstall the Service
### Install
``` bash
sc create "serviceName" binpath="Folder of the .exe"
```

### Start
``` bash
Start-Service -Name "serviceName"
```
### Stop
``` bash
Stop-Service -Name "serviceName"
```

### Uninstall
```shell 
sc delete serviceName
```

