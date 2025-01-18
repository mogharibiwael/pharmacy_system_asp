using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Localization;

namespace PMS.Localization
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, Dictionary<string, string>> _localizationData;
        private readonly string _baseName;

        public JsonStringLocalizer(string resourcesPath, string baseName)
        {
            _baseName = baseName;
            _localizationData = LoadLocalizationData(resourcesPath);
        }

        private Dictionary<string, Dictionary<string, string>> LoadLocalizationData(string resourcesPath)
        {
            var localizationData = new Dictionary<string, Dictionary<string, string>>();

            if (!Directory.Exists(resourcesPath))
                return localizationData;

            foreach (var file in Directory.GetFiles(resourcesPath, "*.json"))
            {
                var culture = Path.GetFileNameWithoutExtension(file);
                var json = File.ReadAllText(file);
                var values = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (values != null)
                {
                    localizationData[culture] = values;
                }
            }

            return localizationData;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var culture = CultureInfo.CurrentUICulture.Name;
                if (_localizationData.ContainsKey(culture) && _localizationData[culture].TryGetValue(name, out var value))
                {
                    return new LocalizedString(name, value);
                }

                return new LocalizedString(name, name, true);
            }
        }

        public LocalizedString this[string name, params object[] arguments] =>
            new LocalizedString(name, string.Format(this[name].Value, arguments));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            _localizationData.ContainsKey(CultureInfo.CurrentUICulture.Name)
                ? _localizationData[CultureInfo.CurrentUICulture.Name].Select(kvp => new LocalizedString(kvp.Key, kvp.Value))
                : new List<LocalizedString>();
    }
}
