using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdt.svc.services;

namespace pdt_svc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        ISearchEngine? _engine;
        IConfiguration Configuration;

        // ctor
        public SearchController(ISearchEngine searchEngine, 
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
                List<SearchResult> results = _engine.Search(term, Convert.ToInt32(Configuration["SearchResultsMax"].ToString()));
                return Ok(results);
            }
            catch 
            {
                return NoContent();
            }
        }
    }
}
