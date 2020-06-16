using GuardameLugar.Common.Dto;
using GuardameLugar.Common.Exceptions;
using GuardameLugar.Common.Extensions;
using GuardameLugar.Common.Helpers;
using GuardameLugar.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;

namespace GuardameLugar.API.Controllers
{
    [Route("guardamelugar/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class GaragesController : ControllerBase
    {
        private readonly ILogger<GaragesController> _logger;
        private readonly IGarageService _garageService;
        public GaragesController(ILogger<GaragesController> logger, IGarageService garageService)
        {
            _logger = logger;
            _garageService = garageService;
        }

        [HttpPost("GarageRegister")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> GarageRegister(GarageRegisterDto garageRegisterDto)
        {
            try
            {
                await _garageService.GarageRegister(garageRegisterDto);
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

        [HttpGet("Localidades")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LocalidadesDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> Localidades()
        {
            try
            {
                List<LocalidadesDto> listaLocalidades = await _garageService.Localidades();
                return Response.Ok(listaLocalidades);
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

        [HttpGet("{garageId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GarageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> GetGarageById(int garageId)
        {
            try
            {
                GarageDto garageDto = await _garageService.GetGarageById(garageId);
                return Response.Ok(garageDto);
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

        [HttpGet("user/{userId}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GarageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> GetGarageByUser(int userId)
        {
            try
            {
                List<GarageDto> garageList = await _garageService.GetGarageByUser(userId);
                return Response.Ok(garageList);
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

        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GarageDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> GetGarages([FromQuery]string vehiculo, [FromQuery]int? localidad)
        {
            try
            {
                List<GarageDto> garageList = await _garageService.GetGarages(vehiculo, localidad);
                return Response.Ok(garageList);
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

        [HttpPut]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(JsonMessage))]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity, Type = typeof(JsonMessage))]
        public async Task<ActionResult> UpdateGarage(UpdateGarageDto updateGarageDto)
        {
            try
            {
                await _garageService.UpdateGarage(updateGarageDto);
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
    }
}