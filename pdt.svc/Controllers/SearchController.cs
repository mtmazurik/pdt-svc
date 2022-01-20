using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdt.svc.services;
using pdt.svc.services.exceptions;

namespace pdt_svc.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class searchController : ControllerBase
    {
        ISearchEngine? _engine;
        ILogger _logger;

        public searchController(  IConfiguration config
                                , ILogger<searchController> logger)
        {
            string cx = config["Secrets:GoogleCustomSearchCx"].ToString();   // read AppSettings: secrets & settings passed from ENV vars
            string apiKey = config["Secrets:GoogleCustomSearchApiKey"].ToString();
            int maxResults = Convert.ToInt32(config["SearchEngine:SearchResultsMax"].ToString());

            _engine = new SearchEngine(maxResults, cx, apiKey);   // NOT using DI here; then easily create constructor w/ ENV params
            _logger = logger;
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
            catch(searchException exc) 
            {
                _logger.LogError(exc.Message + Environment.NewLine + "innerException: " + exc.ToString()); // pattern: message from the service, innerexception of the caught (general) exception
                return NoContent();
            }
        }
    }
}
