namespace PogoVersionMonitor.Utilities
{
    using System;
    using System.Net;
    using System.Threading;

    public static class NetUtils
    {
        /// <summary>
        /// Send a HTTP POST request to the provided url address
        /// </summary>
        /// <param name="url">Web address to send JSON payload to.</param>
        /// <param name="json">JSON payload to send to endpoint.</param>
        public static void SendWebhook(string url, string json)
        {
            using var wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");

            try
            {
                var resp = wc.UploadString(url, json);
                //Console.WriteLine($"Response: {resp}");
                Thread.Sleep(500);
            }
            catch (WebException ex)
            {
                var resp = (HttpWebResponse)ex.Response;
                switch (resp?.StatusCode)
                {
                    //https://discordapp.com/developers/docs/topics/rate-limits
                    case HttpStatusCode.TooManyRequests: // 429
                        Console.WriteLine("RATE LIMITED");
                        var retryAfter = resp.Headers["Retry-After"];
                        //var limit = resp.Headers["X-RateLimit-Limit"];
                        //var remaining = resp.Headers["X-RateLimit-Remaining"];
                        //var reset = resp.Headers["X-RateLimit-Reset"];
                        if (!int.TryParse(retryAfter, out var retry))
                            return;

                        Thread.Sleep(retry);
                        SendWebhook(url, json);
                        break;
                }
            }
        }
    }
}