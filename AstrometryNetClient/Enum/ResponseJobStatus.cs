
using AstrometryNetClient.Json;

namespace AstrometryNetClient.Enum
{
	/// <summary>
	/// Response status received from Astrometry.net after sending a JobStatus request
	/// </summary>
	/// <seealso cref="JobStatusResponse"/>
	public enum ResponseJobStatus
	{
		/// <summary>
		/// the solve succeded
		/// </summary>
		success,
		/// <summary>
		/// unsucessful solve
		/// </summary>
		failure,
		/// <summary>
		/// the image is currently processed
		/// </summary>
		solving,
	}
}
