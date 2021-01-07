using Jurassic.Library;
using Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace Core
{
    public static class IntegrationFactory
    {
        public static List<string> GetLinks(this IntegrationSettings settings)
        {
            switch (settings.Integration)
            {
                case "none":
                    return None(settings.Settings);
                case "dailyiptvlist":
                case "freeiptvlists":
                    return FreeIpLists(settings.Settings);
            }
            return new List<string>();

        }
        public static List<M3U8Channel> GetLinksModelled(this IntegrationSettings settings)
        {
            switch (settings.Integration)
            {
                case "htatv":
                    return HTA(settings.Settings);
                case "elahmad":
                    return Elahmad(settings.Settings);
            }
            return new List<M3U8Channel>();

        }

        private static List<M3U8Channel> Elahmad(Settings settings)
        {
            string decode(string value)
            {
                int mod4 = value.Length % 4;
                if (mod4 > 0)
                {
                    value += new string('=', 4 - mod4);
                }
                return Encoding.UTF8.GetString(Convert.FromBase64String(value));
            }
            string getJWT(string value)
            {
                return Regex.Matches(value, "(1\\(\")(.*)(<?\\\")")[0].Groups[2].Value;
            }
            string parseJWT(string jwt)
            {
                string a = jwt.Split(".")[1].Replace("-", "+").Replace("_", "/");
                var jsonObject = JObject.Parse(decode(a)).SelectToken("link").ToString();

                return jsonObject;

            }
            List<M3U8Channel> channels = new List<M3U8Channel>();
            HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
            foreach (var channel in settings.ChannelLinks)
            {
                var document = htmlWeb.Load(channel.Link);
                var script = document.DocumentNode.Descendants()
                         .Where(n => n.Name == "script" && !string.IsNullOrEmpty(n.InnerText)).FirstOrDefault().InnerText;

                script = script.Replace("\"", "'").Replace("=\"", "=''");
                script = Regex.Replace(script, "(; document).*", ";");
                var outputVariable = Regex.Match(script, @"var (\w) = ''").Value.Replace("var ", "").Replace(@"= ''", "").Trim();

                try
                {
                    // Return the data of spect and stringify it into a proper JSON object
                    var engine = new Jurassic.ScriptEngine();
                    var result = engine.Evaluate("(function() { " + script + " return " + outputVariable + "; })()");
                    string qq = "";
                    if (result.ToString().StartsWith("<video"))
                    {
                        var matches = Regex.Matches(result.ToString(), @"(http|ftp|https):\/\/([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:\/~+#-]*[\w@?^=%&\/~+#-])?");
                        qq = matches[matches.Count - 1].Groups[0].Value;
                    }
                    else
                    {
                        var lastScript = Regex.Match(result.ToString().Replace("\r\n", ""), "function parse.*").Value.Replace(@";</script>", ";");
                        var jwt = getJWT(lastScript);
                        qq = parseJWT(jwt);
                    }

                    channels.Add(new M3U8Channel(channel, "Elahmad", qq));

                }
                catch (Exception ex)
                {

                }
            }
            return channels;
        }

        private static List<M3U8Channel> HTA(Settings settings)
        {
            List<M3U8Channel> channels = new List<M3U8Channel>();
            //string[] codecs = new string[] { "_240p", "_360p" };
            using (HttpClient client = new HttpClient())
            {
                string token = client.GetAsync(settings.AuthToken).Result.Content.ReadAsStringAsync().Result;

                string tvList = client.GetAsync(settings.Links.FirstOrDefault()).Result.Content.ReadAsStringAsync().Result;

                JToken jobject = JObject.Parse(tvList).SelectToken("tiles");

                var list = jobject.ToObject<List<HTAModel>>();

                foreach (var item in list.Where(x => x.channel != null))
                {
                    //foreach (var codec in codecs)
                    //{
                    //    channels.Add(new M3U8Channel(item, codec, token));
                    //}
                    channels.Add(new M3U8Channel(item, token));

                }
            }
            return channels;
        }

        private static List<string> FreeIpLists(Settings settings)
        {
            List<string> links = new List<string>();

            if (settings.HasScheme())
            {
                switch (settings.SchemeType)
                {
                    case "date":
                        if (settings.LastPartIndex != null)
                        {

                            for (int i = 1; i < settings.LastPartIndex; i++)
                            {
                                if (settings.Periods.Contains("yesterday"))
                                {
                                    links.Add(settings.Scheme.Replace("{date}", DateTime.Now.AddDays(-1).ToString(settings.DateFormat)).Replace("{part}", i.ToString()));
                                }
                                if (settings.Periods.Contains("today"))
                                {
                                    links.Add(settings.Scheme.Replace("{date}", DateTime.Now.ToString(settings.DateFormat)).Replace("{part}", i.ToString()));
                                }
                            }
                        }
                        else
                        {
                            if (settings.Periods.Contains("yesterday"))
                            {
                                links.Add(settings.Scheme.Replace("{date}", DateTime.Now.AddDays(-1).ToString(settings.DateFormat)));
                            }
                            if (settings.Periods.Contains("today"))
                            {
                                links.Add(settings.Scheme.Replace("{date}", DateTime.Now.ToString(settings.DateFormat)));
                            }
                        }
                        return links;
                    default:
                        break;
                }
            }
            return links;
        }

        private static List<string> None(Settings settings)
        {
            return settings.Links;
        }

    }
}
