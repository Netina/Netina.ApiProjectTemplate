namespace Server.WebFramework.Configurations
{
    public static class LoggerConfig
    {
        public static void ConfigureSerilog()
        {
            string logName = $"{FilePaths.Logs}/Log_Server_.log";
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.ElmahIo(new ElmahIoSinkOptions("279e90cbf4da4814a971a33df23ebd1e", new Guid("7fb24511-87b1-414e-933a-006f67e97ffc"))
                {
                    MinimumLogEventLevel = LogEventLevel.Warning,

                })
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                .WriteTo.File(logName, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

    }
}
