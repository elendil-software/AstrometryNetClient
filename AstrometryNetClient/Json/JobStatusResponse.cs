using AstrometryNetClient.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AstrometryNetClient.Json
{
	/// <summary>
	/// Result of a job status request
	/// </summary>
	/// <seealso cref="Client.GetJobStatus"/>
	public class JobStatusResponse
	{
		/// <summary>
		/// status of the job
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ResponseJobStatus status { get; set; }
	}
}
