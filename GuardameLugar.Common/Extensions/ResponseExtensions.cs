using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using GuardameLugar.Common.Exceptions;
using GuardameLugar.Common.Helpers;

namespace GuardameLugar.Common.Extensions
{
	public class JsonResultList<T>
	{
		public JsonResultList(List<T> list)
		{
			Result = list;
			Total = list.Count;
		}

		public List<T> Result { get; set; }
		public int Total { get; set; }
	}

	public class JsonResultListRows<T>
	{
		public JsonResultListRows(List<T> list, int TRows)
		{
			Result = list;
			Total = list.Count;
			TotalRows = TRows;
		}

		public List<T> Result { get; set; }
		public int Total { get; set; }
		public int TotalRows { get; set; }
	}

	#region --StatusCodeResult Response Objects--
	public class JsonResultOk : StatusCodeResult
	{
		public JsonResultOk(object obj) : base((int)HttpStatusCode.OK)
		{
			Result = new JsonResult(obj);
			Result.StatusCode = (int)HttpStatusCode.OK;
		}

		public JsonResult Result { get; }

		public StatusCodeResult StatusCodeResult { get { return this; } }
	}

	public class JsonResultCreated : StatusCodeResult
	{
		public JsonResultCreated(object obj) : base((int)HttpStatusCode.Created)
		{
			Result = new JsonResult(obj);
			Result.StatusCode = (int)HttpStatusCode.Created;
		}

		public JsonResult Result { get; }

		public StatusCodeResult StatusCodeResult { get { return this; } }
	}

	public class JsonResultUnprocessableEntity : StatusCodeResult
	{
		public JsonResultUnprocessableEntity(object obj) : base((int)HttpStatusCode.UnprocessableEntity)
		{
			Result = new JsonResult(obj);
			Result.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
		}

		public JsonResult Result { get; }

		public StatusCodeResult StatusCodeResult { get { return this; } }
	}

	public class JsonResultBadRequest : StatusCodeResult
	{
		public JsonResultBadRequest(object obj) : base((int)HttpStatusCode.BadRequest)
		{
			Result = new JsonResult(obj);
			Result.StatusCode = (int)HttpStatusCode.BadRequest;
		}

		public JsonResult Result { get; }

		public StatusCodeResult StatusCodeResult { get { return this; } }
	}

	public class JsonResultInternalServerErrort : StatusCodeResult
	{
		public JsonResultInternalServerErrort(object obj) : base((int)HttpStatusCode.InternalServerError)
		{
			Result = new JsonResult(obj);
			Result.StatusCode = (int)HttpStatusCode.InternalServerError;
		}

		public JsonResult Result { get; }

		public StatusCodeResult StatusCodeResult { get { return this; } }
	}

	public class JsonResultNotFound : StatusCodeResult
	{
		public JsonResultNotFound(object obj) : base((int)HttpStatusCode.NotFound)
		{
			Result = new JsonResult(obj);
			Result.StatusCode = (int)HttpStatusCode.NotFound;
		}

		public JsonResult Result { get; }

		public StatusCodeResult StatusCodeResult { get { return this; } }
	}
	#endregion

	public static class Response
	{
		#region --Ok--
		public static JsonResult Ok(this HttpResponse response, object obj)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			return (new JsonResultOk(obj)).Result;
		}

		public static OkResult Ok(this HttpResponse response)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			return new OkResult();
		}

		public static JsonResult Ok<T>(this HttpResponse response, List<T> list)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			return (new JsonResultOk(new JsonResultList<T>(list))).Result;
		}
		public static JsonResult Ok<T>(this HttpResponse response, List<T> list, int totalRows)
		{
			response.StatusCode = (int)HttpStatusCode.OK;
			return (new JsonResultOk(new JsonResultListRows<T>(list, totalRows))).Result;
		}

		#endregion

		#region --Created--
		public static JsonResult Created(this HttpResponse response, object obj)
		{
			response.StatusCode = (int)HttpStatusCode.Created;
			return (new JsonResultCreated(obj)).Result;
		}
		public static StatusCodeResult Created(this HttpResponse response)
		{
			response.StatusCode = (int)HttpStatusCode.Created;
			return new StatusCodeResult((int)HttpStatusCode.Created);
		}
		#endregion

		public static JsonResult UnprocessableEntity(this HttpResponse response, object obj)
		{
			response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
			return (new JsonResultUnprocessableEntity(obj)).Result;
		}

		public static JsonResult BadRequest(this HttpResponse response, object obj)
		{
			response.StatusCode = (int)HttpStatusCode.BadRequest;
			return (new JsonResultBadRequest(obj)).Result;
		}

		public static JsonResult InternalServerError(this HttpResponse response, string reasonPhrase = null)
		{
			response.StatusCode = (int)HttpStatusCode.InternalServerError;
			if (!string.IsNullOrEmpty(reasonPhrase))
				response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Internal Server Error (" + reasonPhrase + ")";
			return (new JsonResultInternalServerErrort("Internal Server Error")).Result;
		}

		public static JsonResult NotFound(this HttpResponse response, object obj)
		{
			response.StatusCode = (int)HttpStatusCode.NotFound;
			return (new JsonResultNotFound(obj)).Result;
		}

		public static JsonResult HandleExceptions(this HttpResponse response, BaseException e)
		{
			var className = e.GetType().Name;
			switch (className)
			{
				case "UnprocessableException":
					return response.UnprocessableEntity(ExceptionHandlerHelper.ExceptionMessage(e));
				case "BadRequestException":
					return response.BadRequest(ExceptionHandlerHelper.ExceptionMessage(e));
				case "NotFoundException":
					return response.NotFound(ExceptionHandlerHelper.ExceptionMessage(e));
				default:
					return response.InternalServerError();
			}
		}
	}
}
