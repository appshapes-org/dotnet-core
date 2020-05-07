using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace AppShapes.Core.Testing.Service
{
    public class FakeHostingEnvironment : IWebHostEnvironment
    {
        public string ApplicationName { get; set; } = AppDomain.CurrentDomain.FriendlyName;

        public IFileProvider ContentRootFileProvider { get; set; }

        public string ContentRootPath { get; set; } = Environment.CurrentDirectory;

        public string EnvironmentName { get; set; } = "Test";

        public IFileProvider WebRootFileProvider { get; set; }

        public string WebRootPath { get; set; } = Environment.CurrentDirectory;
    }
}