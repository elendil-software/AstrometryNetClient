using System;

namespace software.elendil.AstrometryNet.Exceptions
{
	/// <summary>
	/// Exception that can be raised by the Astrometry.net C# client
	/// </summary>
	[Serializable]
	public class AstrometryNotConnectedException : AstrometryException
	{
		public AstrometryNotConnectedException()
		{
		}

		public AstrometryNotConnectedException(string message) : base(message)
		{
		}

		public AstrometryNotConnectedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}