using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Porotkin_SP_9.Models;

namespace Porotkin_SP_9.Controllers
{
    public class LowLevelFunctionController : Controller
    {
        private readonly ILogger<LowLevelFunctionController> _logger;

        public LowLevelFunctionController(ILogger<LowLevelFunctionController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calculate(LowLevelParams parameters)
        {
            var result = "";
            try
            {
                result =
                    LowLevelFunction.LowLevelFunction.LowLevelMultiplication(parameters.First, parameters.Second)
                        .ToString();
            }
            catch (OverflowException)
            {
                result = "The input values multiplication resulted in an overflow!";
            }
            catch (Exception)
            {
                result = "Something went completely awry";
            }
            _logger.LogInformation(
                $"The result of multiplication of {parameters.First} and {parameters.Second} is {result}");
            return View(new LowLevelResult(result));
        }
    }
}