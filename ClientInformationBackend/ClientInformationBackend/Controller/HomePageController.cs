using ClientInformationBackend.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClientInformationBackend.Core.Controller
{
    [ApiController]
    [Route(ControllerRoute)]
    public class HomePageController : ControllerBase
    {
        private readonly IClientInformationBackendService _ClientInformationBackendService;
        public const string ControllerRoute = "/";
        public HomePageController(IClientInformationBackendService ClientInformationBackendService)
        {
            this._ClientInformationBackendService = ClientInformationBackendService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal))]
        public IActionResult Information()
        {
            return this.Ok(ClientInformationBackendController.CalculateResponseForClientInformationRequest(ClientInformationBackendController.GetClientIPAddress(this.HttpContext), this._ClientInformationBackendService));
        }
    }
}
