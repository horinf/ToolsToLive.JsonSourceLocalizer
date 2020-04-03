using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToolsToLive.JsonSourceLocalizer.Interfaces;

namespace ToolsToLive.JsonSourceLocalizer
{
    /// <inheritdoc />
    public class JsonSourceLocalizer : IJsonSourceLocalizer
    {
        private readonly IJsonResourceManager _resourceManager;

        public JsonSourceLocalizer(
            IJsonResourceManager resourceManager)
        {
            _resourceManager = resourceManager;
        }

        /// <inheritdoc />
        public async Task<string> Localize(string key, string culture)
        {
            string[] keyPath = key.Split('.');

            // dictionary with nested dictionary with nested dictionary with... and so on.
            Dictionary<string, object> namespaceDictionary = await _resourceManager.GetDictionary(culture, keyPath);
            if (namespaceDictionary == null)
            {
                throw new Exception("Unable to find localization namespaces or values");
            }

            if (keyPath.Length > 1)
            {
                object tmp;
                for (int i = 0; i < keyPath.Length - 1; i++)
                {
                    if (!namespaceDictionary.TryGetValue(keyPath[i], out tmp))
                    {
                        throw new Exception($"Unable to find localization namespace: {keyPath[i]}");
                    }

                    namespaceDictionary = tmp as Dictionary<string, object>;
                    if (namespaceDictionary == null)
                    {
                        throw new Exception($"Unable to find localization namespace: {keyPath[i]}");
                    }
                }
            }

            object value;
            string lastKey = keyPath.Last();
            if (!namespaceDictionary.TryGetValue(lastKey, out value))
            {
                throw new Exception($"Localization for key {lastKey} not found");
            }

            return value as string;
        }

        /// <inheritdoc />
        public async Task<string> Localize(string key, string culture, params object[] arguments)
        {
            string value = await Localize(key, culture);
            return string.Format(value, arguments);
        }
    }
}
