namespace PogoVersionMonitor.Extensions
{
    using System;
    using System.IO;
    using System.Text.Json;

    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            //IgnoreReadOnlyProperties = true,
            //DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, _jsonOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj) =>
            JsonSerializer.Serialize(obj, _jsonOptions);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static T LoadFromFile<T>(this string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"{filePath} file not found.", filePath);
            }

            var data = File.ReadAllText(filePath);
            if (string.IsNullOrEmpty(data))
            {
                Console.WriteLine($"'{filePath}' file is empty.");
                return default;
            }

            return data.FromJson<T>();
        }
    }
}