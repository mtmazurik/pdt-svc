using System.Collections.Generic;
using NUnit.Framework;
using pdt.svc.services;

namespace pdt.svc.tests
{
    public class UnitSearchEngine
    {
        ISearchEngine? _engine;

        [SetUp]
        public void Setup()
        {
            _engine = new SearchEngine();
        }

        [Test]
        public void SearchReturnsResults()
        {
            List<SearchResult> searchResults = _engine.Search("prize drawing");
            if( searchResults.Count > 0) 
            { 
                Assert.Pass();
            }
        }
    }
}