using CommandLine;

namespace AppShapes.Core.Console
{
    public class ProgramOptions
    {
        [Option('f', "basePath", Required = false, HelpText = "Set basePath to specified folder.")]
        public virtual string BasePath { get; set; } = System.Environment.CurrentDirectory;

        [Option('e', "environment", Required = false, HelpText = "Set environment to specified identifier.")]
        public virtual string Environment { get; set; } = EnvironmentHelper.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }
}