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
        IConfiguration Configuration;

        // ctor
        public searchController(ISearchEngine searchEngine, 
                                IConfiguration configuration)           
        {
            _engine = searchEngine;
            Configuration = configuration;
        }

        // GET:  /search?term={searchTerm}
        [HttpGet("")]
        public IActionResult Get([FromQuery(Name = "term")]string term)
        {

            try
            {
                string cx = Configuration["SearchEngine:GoogleCustomSearchCx"].ToString();                  // read secrets in from appsettings or env vars during GitHub Actions
                string apiKey = Configuration["SearchEngine:GoogleCustomSearchApiKey"].ToString();
                string queryString = "https://www.googleapis.com/customsearch/v1"
                    + "?key=" + apiKey + "&cx=" + cx + "&q=" + term;

                int maxResultCount = Convert.ToInt32(Configuration["SearchEngine:SearchResultsMax"].ToString());
                List<SearchResult> results = _engine.Search(queryString, maxResultCount);
                return Ok(results);
            }
            catch 
            {
                return NoContent();
            }
        }
    }
}
