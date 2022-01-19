using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdt.svc.services;

namespace pdt_svc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class searchController : ControllerBase
    {
        ISearchEngine? _engine;

        public searchController(IConfiguration config)            // ctor
        {
            string cx = config["Secrets:GoogleCustomSearchCx"].ToString();                  // read secrets in from appsettings or env vars during GitHub Actions
            string apiKey = config["Secrets:GoogleCustomSearchApiKey"].ToString();
            int maxResults = Convert.ToInt32(config["SearchEngine:SearchResultsMax"].ToString());
            _engine = new SearchEngine(maxResults, cx, apiKey);   // more convenient to NOT USE DI here, for parameterized constructors - other patterns, like C#/.Net options pattern is convoluted further; blech, yuk:   more here : https://stackoverflow.com/questions/53884417/net-core-di-ways-of-passing-parameters-to-constructor
                                                                  // don't thing the 'juice is worth the squeeze on this', use good ole 'new'
        }

        // GET:  /search?term={searchTerm}
        [HttpGet("")]
        public IActionResult Get([FromQuery(Name = "query")]string queryString)
        {

            try
            {
                List<SearchResult> results = _engine.Search(queryString);
                return Ok(results);
            }
            catch 
            {

                return NoContent();
            }
        }
    }
}
