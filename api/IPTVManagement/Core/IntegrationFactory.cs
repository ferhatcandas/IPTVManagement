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
                //case "elahmad":
                //    return Elahmad(settings.Settings);
            }
            return new List<M3U8Channel>();

        }

        private static List<M3U8Channel> Elahmad(Settings settings)
        {

            List<M3U8Channel> channels = new List<M3U8Channel>();
            using (HttpClient client = new HttpClient())
            {
                foreach (var link in settings.Links)
                {
                    HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
                    var document = htmlWeb.Load(link);
                    var script = document.DocumentNode.Descendants()
                             .Where(n => n.Name == "script").FirstOrDefault().InnerText;

                    script = script.Replace("\"", "'").Replace("=\"","=''");
                    script = Regex.Replace(script, "(; document).*", ";");
                    var outputVariable = Regex.Match(script, @"var (\w) = ''").Value.Replace("var ", "").Replace(@"= ''", "").Trim();

                    try
                    {
                        // Return the data of spect and stringify it into a proper JSON object
                        var engine = new Jurassic.ScriptEngine();
                        var result = engine.Evaluate("(function() { " + script + " return " + outputVariable + "; })()");

                        var lastScript = Regex.Match(result.ToString().Replace("\r\n", ""), "function parse.*").Value.Replace(@";</script>", ";");

                        result = engine.Evaluate("(function() { " + lastScript + " return streamUrl; })()");
                        var json = JSONObject.Stringify(engine, result);
                    }
                    catch (Exception ex)
                    {

                    }
                    

                    //channels.Add(new M3U8Channel("test", json));
                }
            }
            return channels;
        }

        private static List<M3U8Channel> HTA(Settings settings)
        {
            List<M3U8Channel> channels = new List<M3U8Channel>();
            string[] codecs = new string[] { "_240p", "_360p" };
            using (HttpClient client = new HttpClient())
            {
                string token = client.GetAsync(settings.AuthToken).Result.Content.ReadAsStringAsync().Result;

                string tvList = client.GetAsync(settings.Links.FirstOrDefault()).Result.Content.ReadAsStringAsync().Result;

                JToken jobject = JObject.Parse(tvList).SelectToken("tiles");

                var list = jobject.ToObject<List<HTAModel>>();

                foreach (var item in list.Where(x => x.channel != null))
                {
                    foreach (var codec in codecs)
                    {
                        channels.Add(new M3U8Channel(item, codec, token));
                    }
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
