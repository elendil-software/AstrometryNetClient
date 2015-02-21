using software.elendil.AstrometryNet.Json;

namespace software.elendil.AstrometryNet.Enum
{
	/// <summary>
	/// Response status received from Astrometry.net after sending a JobStatus request
	/// </summary>
	/// <seealso cref="JobStatusResponse"/>
	public enum ResponseJobStatus
	{
		/// <summary>
		/// the solve succeeded
		/// </summary>
		success,

		/// <summary>
		/// unsuccessful solve
		/// </summary>
		failure,

		/// <summary>
		/// the image is currently processed
		/// </summary>
		solving,
	}
}