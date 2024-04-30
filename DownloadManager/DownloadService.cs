namespace DownloadManager
{
    public class DownloadService : BackgroundService
    {
        private readonly ILogger<DownloadService> _logger;
        private readonly IConfigurationRoot _configuration;
        private readonly string videoUrl, docsUrl, exeUrl;

        private readonly string mediaExtensions = ".mp4.flv.png.bmp.jpg.jpeg";
        private readonly string documentExtensions = ".pdf.md.doc.docx.xls.xlsx.ppt.pptx.vsd.vsdx.odt.odp.ods.txt.log";
        private readonly string executableExtensions = ".exe.iso.msi.zip.rar.7z.gz.tar.vsix";

        public DownloadService(ILogger<DownloadService> logger)
        {
            _logger = logger; 
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .AddEnvironmentVariables().Build();
            var urls = _configuration.GetRequiredSection("FolderPaths");
            videoUrl = urls.GetValue<string>("Vids")!;
            docsUrl = urls.GetValue<string>("Docs")!;
            exeUrl = urls.GetValue<string>("Exes")!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Execution startet : {DateTime.Now}");
                try
                {
                    string dlFolder = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads";
                    DirectoryInfo directoryInfo = new DirectoryInfo(dlFolder);
                    foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles().Where(x => mediaExtensions.Contains(x.Extension)))
                    {
                        fileInfo.MoveTo($"{videoUrl}\\{fileInfo.Name}");
                        _logger.LogInformation($"{fileInfo.Name} moved to {videoUrl}");
                    }
                    foreach(FileInfo fileInfo in directoryInfo.EnumerateFiles().Where(x => documentExtensions.Contains(x.Extension)))
                    {
                        fileInfo.MoveTo($"{docsUrl}\\{fileInfo.Name}");
                        _logger.LogInformation($"{fileInfo.Name} moved to {docsUrl}");
                    }
                    foreach (FileInfo fileInfo in directoryInfo.EnumerateFiles().Where(x => executableExtensions.Contains(x.Extension)))
                    {
                        fileInfo.MoveTo($"{exeUrl}\\{fileInfo.Name}");
                        _logger.LogInformation($"{fileInfo.Name} moved to {exeUrl}");
                    }
                    _logger.LogInformation("No more moveable Files found !!!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                await Task.Delay(1000*60*60, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Service started : {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Service ended : {DateTime.Now}");
            return base.StopAsync(cancellationToken);
        }
    }
}
