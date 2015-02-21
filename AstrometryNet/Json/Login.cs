namespace software.elendil.AstrometryNet.Json
{
	/// <summary>
	/// JSON object sent to get a connection
	/// </summary>
	/// <seealso cref="RequestSender.Login"/>
	public class Login
	{
		/// <summary>
		/// Your API Key. You can get it from your Astrometry.net account
		/// </summary>
		public string apikey { get; set; }
	}
}