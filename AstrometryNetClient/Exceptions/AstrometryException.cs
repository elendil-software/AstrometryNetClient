using System;

namespace AstrometryNetClient.Exceptions
{
	/// <summary>
	/// Exception that can be raised by the Astrometry.net C# client
	/// </summary>
	[SerializableAttribute]
	public class AstrometryException : Exception
	{
		public AstrometryException() { }
		public AstrometryException(string message) : base(message) { }
		public AstrometryException(string message, Exception innerException) : base(message, innerException) { }
	}
}
