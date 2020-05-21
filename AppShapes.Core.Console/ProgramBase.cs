using System;
using System.Collections.Generic;
using System.Linq;
using AppShapes.Core.Logging;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppShapes.Core.Console
{
    public abstract class ProgramBase : ProgramBase<ProgramOptions>
    {
        protected ProgramBase(IEnumerable<string> args) : base(args)
        {
        }
    }

    public abstract class ProgramBase<T> : IDisposable where T : ProgramOptions
    {
        private ILogger itsLogger;

        protected ProgramBase(IEnumerable<string> args)
        {
            Parser.Default.ParseArguments<T>(args).WithParsed(OnCommandLineParsed).WithNotParsed(OnCommandLineNotParsed);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Run()
        {
            if (Environment.ExitCode != 0)
                return;
            Initialize();
            StartWork();
            StopWork();
        }

        protected IConfiguration Configuration { get; set; }

        protected virtual void ConfigureServices(IServiceCollection services)
        {
            new ConfigureLoggingCommand().Execute(services, Configuration);
        }

        protected T Context { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            (Provider as IDisposable)?.Dispose();
        }

        protected virtual IConfigurationBuilder GetConfigurationBuilder()
        {
            return new ConfigureConfigurationBuilderCommand().Execute(Context);
        }

        protected virtual ILogger GetLogger()
        {
            return Provider.GetRequiredService<ILogger<ProgramBase<T>>>();
        }

        protected virtual IServiceCollection GetServiceProviderBuilder()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            return services;
        }

        protected virtual void Initialize()
        {
            Configuration = GetConfigurationBuilder().Build();
            Provider = GetServiceProviderBuilder().BuildServiceProvider();
        }

        protected ILogger Logger => itsLogger ??= GetLogger();

        protected virtual void OnCommandLineNotParsed(IEnumerable<Error> errors)
        {
            Error error = errors?.FirstOrDefault();
            Environment.ExitCode = error == null ? 1 : (int) error.Tag;
        }

        protected virtual void OnCommandLineParsed(T context)
        {
            Environment.ExitCode = 0; // TODO: We may be able to remove this line if ExitCode initializes to 0.
            Context = context;
        }

        protected IServiceProvider Provider { get; set; }

        protected virtual void StartWork()
        {
            Logger.Information<ProgramBase<T>>("Start work");
        }

        protected virtual void StopWork()
        {
            Logger.Information<ProgramBase<T>>("Stop work");
        }
    }
}