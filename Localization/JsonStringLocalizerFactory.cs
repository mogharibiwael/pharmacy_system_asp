using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace PMS.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly string _resourcesPath;

        public JsonStringLocalizerFactory(IOptions<JsonLocalizationOptions> options)
        {
            _resourcesPath = options.Value.ResourcesPath ?? "Resources";
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            return new JsonStringLocalizer(_resourcesPath, resourceSource.Name);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new JsonStringLocalizer(_resourcesPath, baseName);
        }
    }

    public class JsonLocalizationOptions
    {
        public string ResourcesPath { get; set; } = "Resources";
    }
}
