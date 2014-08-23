
using AstrometryNetClient.Json;

namespace AstrometryNetClient.Enum
{
	/// <summary>
	/// Response status received from Astrometry.net after sending a request
	/// </summary>
	/// <seealso cref="LoginResponse"/>
	/// <seealso cref="SubmissionImagesResponse"/>
	/// <seealso cref="UploadResponse"/>
	public enum ResponseStatus
	{
		/// <summary>
		/// returned in cas of unsuccesful request
		/// </summary>
		error,
		/// <summary>
		/// returned if the request succeded
		/// </summary>
		success,
	}
}
