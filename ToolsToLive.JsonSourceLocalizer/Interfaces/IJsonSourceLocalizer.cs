using System.Threading.Tasks;

namespace ToolsToLive.JsonSourceLocalizer.Interfaces
{
    /// <summary>
    /// Localizer that uses JSON file as a source for term localizations.
    /// </summary>
    public interface IJsonSourceLocalizer
    {
        /// <summary>
        /// Localizes by key and culture.
        /// </summary>
        /// <param name="key">Key, devided by dots if necessary (localization namespaces).</param>
        /// <param name="culture">Culture to search files according to ("en" culture -- search for "en.json" file).</param>
        /// <returns>Localized term.</returns>
        Task<string> Localize(string key, string culture);

        /// <summary>
        /// Localizes by key and culture.
        /// </summary>
        /// <param name="key">Key, devided by dots if necessary (localization namespaces).</param>
        /// <param name="culture">Culture to search files according to ("en" culture -- search for "en.json" file).</param>
        /// <param name="arguments">Arguments to include to localization term.</param>
        /// <returns>Localized term.</returns>
        Task<string> Localize(string key, string culture, params object[] arguments);
    }
}
