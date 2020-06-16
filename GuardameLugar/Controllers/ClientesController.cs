using GuardameLugar.Core;
using System.Threading.Tasks;
using GuardameLugar.Common;
using System;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using GuardameLugar.Common.Extensions;
using GuardameLugar.Common.Exceptions;
using GuardameLugar.Common.Helpers;
using System.Net.Mime;
using GuardameLugar.Common.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace GuardameLugar.API.Controllers
{
    [Route("guardamelugar/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clientesService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clientesService, ILogger<ClientesController> logger)
        {
            _clientesService = clientesService;
            _logger = logger;
        }

        
        [HttpPost("signup")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> SaveUser(UserDto userDto)
        
        {
            try
            {
                await _clientesService.SaveUser(userDto);
                return Response.Ok();
            }
            catch (BaseException e)
            {
                _logger.LogInformation(ExceptionHandlerHelper.ExceptionMessageStringToLogger(e));
                return Response.HandleExceptions(e);
            }
            catch (Exception e)
            {
                _logger.LogError(e, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
                return Response.InternalServerError();
            }
        }

        [HttpGet("login/{user}/{password}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LogInDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> LogInUser(string user, string password)
        {
            try
            {
                LogInDto objUser = await _clientesService.LogInUser(user, password);
                return Response.Ok(objUser);
            }
            catch (BaseException e)
            {
                _logger.LogInformation(ExceptionHandlerHelper.ExceptionMessageStringToLogger(e));
                return Response.HandleExceptions(e);
            }
            catch (Exception e)
            {
                _logger.LogError(e, GetType().Name + "." + MethodBase.GetCurrentMethod().Name);
                return Response.InternalServerError();
            }
        }
    }
}