using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.IO;


namespace pdt.svc.services
{

    public record SearchResult
    {
        public string? Title { get; set; }
        public string? Link { get; set; }
        public string? Snippet { get; set; }
        public string? Source { get; set; }
        public string? Query { get; set; }
        public string? Index { get; set; }
    }

    public class SearchEngine : ISearchEngine
    {
        private int _maxResults;
        private string _cx;
        private string _apiKey;
        public SearchEngine(int maxResults, string cx, string apiKey)   // ctor
        {
            _maxResults = maxResults;      // appsettings construction via injectsion, some settings
            _cx = cx;
            _apiKey = apiKey;
        }
        public List<SearchResult> Search(string querySubString)
        {
            var results = new List<SearchResult>();

            string queryString = "https://www.googleapis.com/customsearch/v1"
                + "?key=" + _apiKey + "&cx=" + _cx + "&q=" + HttpUtility.UrlEncode(querySubString); ;

            string tenPerPageQueryString = "";
            for (int i = 0; i < _maxResults; i = i + 10)
            {

#pragma warning disable SYSLIB0014 // Type or member is obsolete
                tenPerPageQueryString = queryString + "&start=" + i;      // need to offset by i each iteration
                var request = WebRequest.Create(tenPerPageQueryString);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader r = new StreamReader(s);
                string responseString = r.ReadToEnd();
                dynamic? jsonResponse = JsonConvert.DeserializeObject(responseString);
                foreach (var item in jsonResponse.items)
                {
                    results.Add(new SearchResult
                    {
                        Title = item.title,
                        Link = item.link,
                        Snippet = item.snippet,
                    });
                }
            }
            return results;
        }
    }
}
