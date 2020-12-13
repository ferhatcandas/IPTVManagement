using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class FetchLink
    {
        public string Link { get; set; }
        public DateTime? LastFetch { get; set; }
        public string Scheme { get; set; }
        public bool HasScheme() => !string.IsNullOrEmpty(Scheme);
        public int? LastPartIndex { get; set; }
        public string SchemeType { get; set; }
        public string Dates { get; set; }
        public string DateFormat { get; set; }
        public string[] GetLinks()
        {
            List<string> links = new List<string>();

            if (HasScheme())
            {
                switch (SchemeType)
                {
                    case "date":
                        if (LastPartIndex != null)
                        {

                            for (int i = 1; i < LastPartIndex; i++)
                            {
                                if (Dates.Contains("yesterday"))
                                {
                                    links.Add(Scheme.Replace("{date}", DateTime.Now.AddDays(-1).ToString(DateFormat)).Replace("{part}", i.ToString()));
                                }
                                if (Dates.Contains("today"))
                                {
                                    links.Add(Scheme.Replace("{date}", DateTime.Now.ToString(DateFormat)).Replace("{part}", i.ToString()));
                                }
                            }
                        }
                        else
                        {
                            if (Dates.Contains("yesterday"))
                            {
                                links.Add(Scheme.Replace("{date}", DateTime.Now.AddDays(-1).ToString(DateFormat)));
                            }
                            if (Dates.Contains("today"))
                            {
                                links.Add(Scheme.Replace("{date}", DateTime.Now.ToString(DateFormat)));
                            }
                        }
                        return links.ToArray();
                    default:
                        break;
                }
            }
            links.Add(Link);
            return links.ToArray();
        }
    }
}
