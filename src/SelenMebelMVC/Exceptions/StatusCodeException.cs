using System.Net;

namespace SelenMebelMVC.Exceptions
{
	public class StatusCodeException : Exception
	{
		public HttpStatusCode StatusCode { get; set; }
		public StatusCodeException()
		{

		}
		public StatusCodeException(HttpStatusCode statusCode, string message) :
			base(message)
		{
			StatusCode = statusCode;
		}
	}
}
