using GuardameLugar.Common.Exceptions;

namespace GuardameLugar.Common.Helpers
{
	public class JsonMessage
	{
		public string Message { get; set; }

		public JsonMessage(string message)
		{
			Message = message;
		}
	}
	public static class ExceptionHandlerHelper
	{
		public static object ExceptionMessage(BaseException e, string messaje = null)
		{
			if (string.IsNullOrEmpty(messaje))
				messaje = e.Message;
			return ExceptionMessage(messaje, e.ErrorCode);
		}

		public static object ExceptionMessage(string message, int errorCode)
		{
			return new JsonMessage(message);
		}

		public static string ExceptionMessageStringToLogger(BaseException e, string messaje = null)
		{
			if (string.IsNullOrEmpty(messaje))
				messaje = e.Message;
			return $"{e.ErrorCode}-{messaje}";
		}

	}
}
