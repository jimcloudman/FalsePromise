using System;

namespace FalsePromise.Router.Models
{
	public class RouterException : Exception
	{
		public RouterException() { }

		public RouterException(string message) : base(message) { }

		public RouterException(string message, Exception ex) : base(message, ex) { }
	}
}
