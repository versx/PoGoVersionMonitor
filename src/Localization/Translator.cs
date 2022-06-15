namespace PogoVersionMonitor.Localization
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using PogoVersionMonitor.Diagnostics;

    public class Translator : Language<string, string, Dictionary<string, string>>
    {
        private static readonly string _binLocalesFolder = Directory.GetCurrentDirectory() + $"/{Strings.BasePath}/{Strings.LocaleFolder}";

        private readonly IEventLogger _logger = new EventLogger(Program.OnLogEvent);

        #region Singleton

        private static Translator _instance;

        public static Translator Instance =>
            _instance ??= new Translator
            {
                LocaleDirectory = _binLocalesFolder,
                //CurrentCulture = 
            };

        #endregion

        public override string Translate(string value)
        {
            try
            {
                return base.Translate(value) ?? value;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to find locale translation for key '{value}'");
                _logger.Error(ex);
            }
            return value;
        }

        public string Translate(string value, params object[] args)
        {
            try
            {
                var text = args?.Length > 0
                    ? string.Format(base.Translate(value), args)
                    : base.Translate(value);
                return text ?? value;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to find locale translation for key '{value}' and arguments: '{string.Join(",", args)}'");
                _logger.Error(ex);
            }
            return value;
        }
    }
}