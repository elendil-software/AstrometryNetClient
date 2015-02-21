using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using software.elendil.AstrometryNet.Enum;

namespace software.elendil.AstrometryNet.Json
{
	/// <summary>
	/// Result of a job status request
	/// </summary>
	/// <seealso cref="RequestSender.GetJobStatus"/>
	public class JobStatusResponse
	{
		/// <summary>
		/// status of the job
		/// </summary>
		[JsonConverter(typeof (StringEnumConverter))]
		public ResponseJobStatus status { get; set; }
	}
}