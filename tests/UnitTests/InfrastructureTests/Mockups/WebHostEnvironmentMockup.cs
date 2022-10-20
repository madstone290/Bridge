using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace Bridge.UnitTests.InfrastructureTests.Mockups
{
    public class WebHostEnvironmentMockup : IWebHostEnvironment
    {
        public string WebRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath
        {
            get;
            set;
        } = string.Empty;
        public string EnvironmentName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
