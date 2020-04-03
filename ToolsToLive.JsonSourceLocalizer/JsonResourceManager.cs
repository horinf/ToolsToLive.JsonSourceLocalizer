using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToolsToLive.JsonSourceLocalizer.Interfaces;

namespace ToolsToLive.JsonSourceLocalizer
{
    /// <summary>
    /// Manager to load dictionary from JSON file.
    /// Internal interface and implementation. Use it only to set up dependency injection, do not inject it directly.
    /// </summary>
    public class JsonResourceManager : IJsonResourceManager
    {
        private readonly JsonSourceLocalizerSettings _settings;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="settings">Localizer settings.</param>
        public JsonResourceManager(JsonSourceLocalizerSettings settings)
        {
            _settings = settings;
        }
        
        /// <inheritdoc />
        public async Task<Dictionary<string, object>> GetDictionary(string culture, string[] keyPath)
        {
            string root = _settings.ResourceDirectoryPath;

            int iteration = 0;
            FileInfo file;

            do
            {
                string path = Path.Combine(root, $"{culture}.json");
                file = new FileInfo(path);
                if (file.Exists)
                {
                    break;
                }

                root = Path.Combine(root, keyPath[iteration]);
                iteration++;

            } while (iteration < keyPath.Length);

            if (!file.Exists)
            {
                throw new Exception($"File in the root foler {root} (including subfolders according to key namespaces) with localization resourses not found");
            }

            string json;
            try
            {
                using (FileStream fileStream = file.OpenRead())
                {
                    using (var reader = new StreamReader(fileStream))
                    {
                        json = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to read localization file (file exists but an error occurred while attempt to read content)", ex);
            }

            try
            {
                var obj = Deserialize(json) as Dictionary<string, object>;
                return obj;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to parse json file with localization", ex);
            }
        }

        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        private static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }
    }
}
