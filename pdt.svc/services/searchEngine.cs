using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using pdt.svc.services.exceptions;

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
        private string _queryString;
        private IMessageProducer? _messageProducer;

        public SearchEngine(int maxResults, string cx, string apiKey, IMessageProducer? messageProducer = null )   // ctor
        {
            _maxResults = maxResults;      // Object-Oriented: encapsulation-via-construction; needed App settings. Note: overrules injection as its complicated to inject parms, this'll be new 'ed
            _cx = cx;
            _apiKey = apiKey;
            _messageProducer = messageProducer;
        }
        public List<SearchResult> Search(string querySubString)
        {
            try
            {
                var results = new List<SearchResult>();

                _queryString = "https://www.googleapis.com/customsearch/v1"
                    + "?key=" + _apiKey + "&cx=" + _cx + "&q=" + HttpUtility.UrlEncode(querySubString); ;

                string tenPerQueryString = "";
                for (int i = 0; i < _maxResults; i = i + 10)
                {

#pragma warning disable SYSLIB0014 // Type or member is obsolete
                    tenPerQueryString = _queryString + "&start=" + i;      // need to offset by 10 each iteration
                    var request = WebRequest.Create(tenPerQueryString);
#pragma warning restore SYSLIB0014 // Type or member is obsolete

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = response.GetResponseStream();
                    StreamReader r = new StreamReader(s);
                    string responseString = r.ReadToEnd();
                    dynamic? jsonResponse = JsonConvert.DeserializeObject(responseString);
                    foreach (var item in jsonResponse.items)
                    {

                        SearchResult sr = new SearchResult
                        {
                            Title = item.title,
                            Link = item.link,
                            Snippet = item.snippet,
                        };
                        results.Add(sr);
                        if( _messageProducer != null)
                        { 
                            _messageProducer.Write(JsonConvert.SerializeObject(sr));  // push each search result to kafka topic
                        } 
                    }
                }
                return results;
            }
            catch (Exception exc)
            {
                throw new searchException("pdt.svc.services.searchengine.search error. querystring=" + _queryString, exc );
            }
        }
    }
}
