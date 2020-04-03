using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToolsToLive.JsonSourceLocalizer.Interfaces
{
    /// <summary>
    /// Manager to load dictionary from JSON file.
    /// Internal interface and implementation. Use it only to set up dependency injection, do not inject it directly.
    /// </summary>
    public interface IJsonResourceManager
    {
        /// <summary>
        /// Gets dictionary from JSON file (parses json objects to nested dictionaries).
        /// </summary>
        /// <param name="culture">Culture to search files according to ("en" culture -- search for "en.json" file)</param>
        /// <param name="keyPath">Key devided by dots (localization namespaces).</param>
        Task<Dictionary<string, object>> GetDictionary(string culture, string[] keyPath);
    }
}
