using software.elendil.AstrometryNet.Json;

namespace software.elendil.AstrometryNet.Enum
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
		/// returned in cas of unsuccessful request
		/// </summary>
		error,

		/// <summary>
		/// returned if the request succeeded
		/// </summary>
		success,
	}
}