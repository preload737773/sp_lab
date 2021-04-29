using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Porotkin_SP_9.Models;

namespace Porotkin_SP_9.Controllers
{
    public class AnalyzerController : Controller
    {
        private readonly ILogger<AnalyzerController> _logger;

        public AnalyzerController(ILogger<AnalyzerController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Analyze(AnalyzerParams parameters)
        {
            var analyzer = new Analyzer.IfAnalyzer();
            _logger.LogDebug($"Inout if-statement: {parameters.IfBodyText}");
            var result = analyzer.FindOutExecutedStatement(parameters.IfBodyText);
            _logger.LogDebug($"Analyzer output message: {result}");
            return View(new AnalyzerResult(result));
        }
    }
}