using System.Net;

using Microsoft.AspNetCore.Mvc;

namespace SystemAggregator.Site.Exceptions
{
    public static class ExceptionGenerator
    {
        public static IActionResult GetServiceUnavailableResult()
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Internal Server Error",
                Detail = "The service is unavailable"
            };

            return new ObjectResult(problemDetails)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
