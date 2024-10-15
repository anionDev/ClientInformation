using ClientInformationBackend.Core.Services;
using GRYLibrary.Core.APIServer.Settings.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClientInformationBackend.Core.Constants;
using System.Net;
using GRYLibrary.Core.Exceptions;
using System.Text.Json;

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
        [Route($"{nameof(this.Information)}")]
        public IActionResult Information()
        {
            return this.Information(this.GetClientIPAddress());
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal))]
        [Route($"{nameof(this.Information)}/{{{nameof(ip)}}}")]
        public IActionResult Information([FromRoute] string ip)
        {
            Model.ClientInformationBackendRecord result = this._ClientInformationBackendService.GetClientInformationBackend(ip);
            return this.Ok(JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                WriteIndented = true
            }));
        }

        private string GetClientIPAddress()
        {
            IPAddress? value = (IPAddress?)this.HttpContext.Items["ClientIPAddress"];
            if (value == null)
            {
                throw new BadRequestException($"Client IP-address not retrievable.");
            }
            else
            {
                return value!.ToString();
            }
        }
    }
}
