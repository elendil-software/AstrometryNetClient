using System;

namespace software.elendil.AstrometryNet.Exceptions
{
	/// <summary>
	/// Exception that can be raised by the Astrometry.net C# client
	/// </summary>
	[Serializable]
	public class AstrometryException : Exception
	{
		public AstrometryException()
		{
		}

		public AstrometryException(string message) : base(message)
		{
		}

		public AstrometryException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}