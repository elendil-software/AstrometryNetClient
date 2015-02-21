using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using software.elendil.AstrometryNet.Enum;

namespace software.elendil.AstrometryNet.Json
{
	/// <summary>
	/// Response of a <see cref="RequestSender.getSubmissionImages">getSubmissionImages</see> request
	/// </summary>
	public class SubmissionImagesResponse
	{
		/// <summary>
		/// status of the request
		/// </summary>
		[JsonConverter(typeof (StringEnumConverter))]
		public ResponseStatus status { get; set; }

		/// <summary>
		/// List of images ids
		/// </summary>
		public string[] image_ids { get; set; }

		/// <summary>
		/// Error message in cas of unsuccessful request
		/// </summary>
		public string errormessage { get; set; }
	}
}