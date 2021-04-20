using Core.Integrations.Abstract;
using Model;
using Model.Integration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Repository.Mongo.Abstract;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Core.Integrations.Concrete
{
    public class ElahmadStrategy : IIntegration
    {
        private readonly IIntegrationRepository<ElahmadSettings> repository;

        public ElahmadStrategy(
            IIntegrationRepository<ElahmadSettings> repository
            )
        {
            this.repository = repository;
        }
        public async Task<List<CommonChannelModel>> GetAsync()
        {
            var setting = await repository.GetFirstAsync();
            var settings = setting.Settings;
            List<CommonChannelModel> channels = new List<CommonChannelModel>();
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

                    channels.Add(new CommonChannelModel
                    {
                        Category = "Elahmad",
                        Country = "AR",
                        Integration = IntegrationType.Full.ToString(),
                        IsEditable = false,
                        HasStream = true,
                        IsActive = true,
                        Language = "Arabic",
                        Logo = channel.Logo,
                        Name = channel.Name,
                        Stream = qq,
                        Tags = null
                    });

                }
                catch (Exception ex)
                {

                }
            }
            return channels;


        }
        private string decode(string value)
        {
            int mod4 = value.Length % 4;
            if (mod4 > 0)
            {
                value += new string('=', 4 - mod4);
            }
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
        private string getJWT(string value)
        {
            return Regex.Matches(value, "(1\\(\")(.*)(<?\\\")")[0].Groups[2].Value;
        }
        private string parseJWT(string jwt)
        {
            string a = jwt.Split(".")[1].Replace("-", "+").Replace("_", "/");
            var jsonObject = JObject.Parse(decode(a)).SelectToken("link").ToString();

            return jsonObject;
        }
    }
}
