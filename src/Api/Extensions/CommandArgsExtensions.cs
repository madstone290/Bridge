
namespace Bridge.Api.Extensions
{
    public static class CommandArgsExtensions
    {
        /// <summary>
        /// 커맨드라인 명령 인자를 처리한다.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="args"></param>
        public static void HandleArgs(this ConfigurationManager configuration, string[] args)
        {
            var colorArg = args.FirstOrDefault(x => x.StartsWith("Color=", StringComparison.OrdinalIgnoreCase));
            if (colorArg != null)
            {
                var color = colorArg.Substring(colorArg.IndexOf('=') + 1);
                configuration.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("DevOpsColor", color)
                });
            }
            
            var uploadDirectoryArg = args.FirstOrDefault(x => x.StartsWith("UploadDirectory=", StringComparison.OrdinalIgnoreCase));
            if (uploadDirectoryArg == null)
                throw new Exception("Upload directory must be provided");

            var uploadDirectory = uploadDirectoryArg.Substring(uploadDirectoryArg.IndexOf('=') + 1);
            configuration.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("UploadDirectory", uploadDirectory)
                });
        }
    }
}
