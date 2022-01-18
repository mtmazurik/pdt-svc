using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using pdt.svc.services;

namespace pdt.svc.tests
{
    public class Tests
    {
        ISearchEngine? _searchEngine;

        public static IConfiguration InitConfiguration()    // copying appsettings.Development.json into the test project
        {                           
            var config = new ConfigurationBuilder()         // then allows using the same secrets from .Development version
                 .AddJsonFile("appsettings.Development.json")// which is NEVER CHECKED IN  because of a correct .gitignore
                 .AddEnvironmentVariables()
                 .Build();
            return config;                                  
        }

        [SetUp]
        public void Setup()
        {
            IConfiguration Configuration = InitConfiguration();  // glommed from: https://stackoverflow.com/questions/39791634/read-appsettings-json-values-in-net-core-test-project
            string cx = Configuration["Secrets:GoogleCustomSearchCx"].ToString();        // read secrets in from appsettings or env vars during GitHub Actions
            string apiKey = Configuration["Secrets:GoogleCustomSearchApiKey"].ToString();
            int maxResults = Convert.ToInt32(Configuration["SearchEngine:SearchResultsMax"].ToString());
            _searchEngine = new SearchEngine(maxResults, cx, apiKey);
        }

        [Test]
        public void SearchReturnsResults()
        {
            List<SearchResult> searchResults = _searchEngine.Search("prize");
            if( searchResults.Count > 0) 
            { 
                Assert.Pass();
            }
        }
    }
}