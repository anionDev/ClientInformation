using ClientInformation.Core.Services;
using GRYLibrary.Core.APIServer.Settings.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientInformation.Core.Constants;

namespace ClientInformation.Core.Controller
{
    [ApiController]
    [Route(ControllerRoute)]
    public class ClientInformationController : ControllerBase
    {
        private readonly IClientInformationService _ClientInformationService;
        public const string ControllerRoute = $"{ServerConfiguration.APIRoutePrefix}/v{GeneralConstants.CodeUnitMajorVersion}/{nameof(ClientInformationController)}";
        public ClientInformationController(IClientInformationService clientInformationService)
        {
            this._ClientInformationService = clientInformationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal))]
        [Route($"{nameof(this.Information)}/{{{nameof(ip)}}}")]
        public IActionResult Information([FromRoute] string ip)
        {
            var result = this._ClientInformationService.GetClientInformation(ip);
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
