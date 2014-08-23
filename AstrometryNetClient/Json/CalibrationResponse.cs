
namespace AstrometryNetClient.Json
{
	/// <summary>
	/// Result of a calibration request
	/// </summary>
	/// <seealso cref="Client.GetCalibration"/>
	public class CalibrationResponse
	{
		/// <summary>
		/// declination
		/// </summary>
		public double dec { get; set; }
		/// <summary>
		/// Radius of the field
		/// </summary>
		public double radius { get; set; }
		/// <summary>
		/// Right ascension
		/// </summary>
		public double ra { get; set; }
		/// <summary>
		/// Orientation of the field
		/// </summary>
		public double orientation { get; set; }
		/// <summary>
		/// Pixel scale
		/// </summary>
		public double pixscale { get; set; }
		/// <summary>
		/// Error message
		/// </summary>
		public string error { get; set; }
	}
}
