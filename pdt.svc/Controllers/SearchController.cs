using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using pdt.svc.services;
using pdt.svc.services.exceptions;

namespace pdt_svc.Controllers
{
    [Route("search")]  
    [ApiController]
    public class searchController : ControllerBase
    {
        ISearchEngine? _engine;
        ILogger _logger;

        public searchController(  IConfiguration config
                                , ILogger<searchController> logger)
        {
            string cx = config["Secrets:Google:CustomSearchCx"].ToString();   // read AppSettings: secrets & settings passed from ENV vars
            string apiKey = config["Secrets:Google:CustomSearchApiKey"].ToString();
            int maxResults = Convert.ToInt32(config["SearchEngine:SearchResultsMax"].ToString());
            bool sendSearchResultsToKafka = Convert.ToBoolean(config["SearchEngine:SendSearchResultsToKafka"]);
            IMessageProducer? _messageProducer = null;

            if(sendSearchResultsToKafka)
            {
                string _kafkaBootstrapServers = config["Secrets:Kafka:BootstrapServers"].ToString();
                string _kafkaSaslUsername = config["Secrets:Kafka:SaslUsername"].ToString();
                string _kafkaSaslPassword = config["Secrets:Kafka:SaslPassword"].ToString();
                _messageProducer = new MessageProducer(_kafkaBootstrapServers, _kafkaSaslUsername, _kafkaSaslPassword);
            }

            _engine = new SearchEngine(maxResults, cx, apiKey, _messageProducer);   // NOT using DI; constructor gets ENV params easier than with DI
            _logger = logger;
        }

        [HttpGet("")]       // usage: /search?query=term+term1+term2     using UrlEncoding, "+" plus signs
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
