using Halcyon.HAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Halcyon.Web.HAL {
    public static class HALResponseExtensions {        
        public static IActionResult ToActionResult(this HALResponse model, ControllerBase controller, HttpStatusCode statusCode = HttpStatusCode.OK) {
            return new ObjectResult(model) {
                StatusCode = (int)statusCode
            };
        }
    }
}
