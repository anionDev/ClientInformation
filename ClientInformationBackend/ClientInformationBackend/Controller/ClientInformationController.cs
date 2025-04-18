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
        internal static readonly JsonSerializerOptions _JSONSettings = new JsonSerializerOptions
        {
            WriteIndented = true
        };
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
            return this.Information(GetClientIPAddress(this.HttpContext));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(decimal))]
        [Route($"{nameof(this.Information)}/{{{nameof(ip)}}}")]
        public IActionResult Information([FromRoute] string ip)
        {
            return this.Ok(CalculateResponseForClientInformationRequest(ip, this._ClientInformationBackendService));
        }
        internal static string CalculateResponseForClientInformationRequest(string ip, IClientInformationBackendService clientInformationBackendService)
        {
            Model.ClientInformationBackendRecord result = clientInformationBackendService.GetClientInformationBackend(ip);
            return JsonSerializer.Serialize(result, _JSONSettings);
        }

        internal static string GetClientIPAddress(HttpContext httpContext)
        {
            IPAddress? value = (IPAddress?)httpContext.Items["ClientIPAddress"];
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
