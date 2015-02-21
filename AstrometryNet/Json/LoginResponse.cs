using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using software.elendil.AstrometryNet.Enum;

namespace software.elendil.AstrometryNet.Json
{
	/// <summary>
	/// Result of a <see cref="RequestSender.Login">Login</see> request
	/// </summary>
	public class LoginResponse
	{
		/// <summary>
		/// Status of the connection
		/// </summary>
		[JsonConverter(typeof (StringEnumConverter))]
		public ResponseStatus status { get; set; }

		/// <summary>
		/// Message in case of successful login
		/// </summary>
		public string message { get; set; }

		/// <summary>
		/// Error message in case of unsuccessful login
		/// </summary>
		public string errormessage { get; set; }

		/// <summary>
		/// session ID
		/// </summary>
		public string session { get; set; }
	}
}