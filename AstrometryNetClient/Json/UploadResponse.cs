using AstrometryNetClient.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AstrometryNetClient.Json
{
	/// <summary>
	/// Response of an <see cref="Client.Upload">Upload</see> request
	/// </summary>
	public class UploadResponse
	{
		/// <summary>
		/// Status of the request
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ResponseStatus status { get; set; }
		/// <summary>
		/// Submission id
		/// </summary>
		public string subid { get; set; }
		/// <summary>
		/// hash code
		/// </summary>
		public string hash { get; set; }
		/// <summary>
		/// Error message in cas of unsuccessful request
		/// </summary>
		public string errormessage { get; set; }
	}
}