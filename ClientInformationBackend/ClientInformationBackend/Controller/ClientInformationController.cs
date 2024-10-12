using ClientInformationBackend.Core.Services;
using GRYLibrary.Core.APIServer.Settings.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientInformationBackend.Core.Constants;

namespace ClientInformationBackend.Core.Controller
{
    [ApiController]
    [Route(ControllerRoute)]
    public class ClientInformationBackendController : ControllerBase
    {
        private readonly IClientInformationBackendService _ClientInformationBackendService;
        public const string ControllerRoute = $"{ServerConfiguration.APIRoutePrefix}/v{GeneralConstants.CodeUnitMajorVersion}/{nameof(ClientInformationBackendController)}";
        public ClientInformationBackendController(IClientInformationBackendService ClientInformationBackendService)
        {
            this._ClientInformationBackendService = ClientInformationBackendService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal))]
        [Route($"{nameof(this.Information)}/{{{nameof(ip)}}}")]
        public IActionResult Information([FromRoute] string ip)
        {
            Model.ClientInformationBackendRecord result = this._ClientInformationBackendService.GetClientInformationBackend(ip);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal))]
        [Route($"{nameof(this.Information)}")]
        public IActionResult Information()
        {
            return this.Information(this.GetIP());
        }
        private string GetIP()
        {
            string result = this.Request.HttpContext.Connection.RemoteIpAddress.ToString();
            //On localhost for example this function returns "::ffff:127.0.0.1". 
            //TODO extract real "plain"-ip-address
            return result;
        }
    }
}
