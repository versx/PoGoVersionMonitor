namespace PogoVersionMonitor.Localization
{
    using System;
    using System.Collections.Generic;

    using PogoVersionMonitor.Diagnostics;

    public class Translator : Language<string, string, Dictionary<string, string>>
    {
        #region Variables

        private static readonly string _binLocalesFolder = $"{Strings.BasePath}/{Strings.LocaleFolder}";
        private readonly IEventLogger _logger = new EventLogger(Program.OnLogEvent);

        #endregion

        #region Singleton

        private static Translator _instance;

        public static Translator Instance =>
            _instance ??= new Translator
            {
                LocaleDirectory = _binLocalesFolder,
                //CurrentCulture = 
            };

        #endregion

        #region Public Methods

        /// <summary>
        /// Translates the provided value by looking up the locale key in the
        /// mapped locale dictionary.
        /// </summary>
        /// <param name="key">The locale key to lookup.</param>
        /// <param name="args">Optional array of arguments to use that replaces
        /// placeholders in the looked up translation.</param>
        /// <returns>Returns the translation string associated with the locale
        /// key. Optionally, placeholder text will be replaced with provided
        /// arguments.</returns>
        public string Translate(string key, params object[] args)
        {
            try
            {
                var text = args?.Length > 0
                    ? string.Format(base.Translate(key), args)
                    : base.Translate(key);
                return text ?? key;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to find locale translation for key '{key}' and arguments: '{string.Join(",", args)}'");
                _logger.Error(ex);
            }
            return key;
        }

        #endregion
    }
}