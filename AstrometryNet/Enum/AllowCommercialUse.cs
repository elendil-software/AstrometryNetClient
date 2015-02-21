using software.elendil.AstrometryNet.Json;

namespace software.elendil.AstrometryNet.Enum
{
	/// <summary>
	/// Indicates if the image can be used for commercial uses. See http://nova.astrometry.net/upload for more details
	/// </summary>
	/// <seealso cref="UploadArgs.allow_commercial_use"/>
	public enum AllowCommercialUse
	{
		/// <summary>
		/// Default
		/// </summary>
		d,

		/// <summary>
		/// Yes
		/// </summary>
		y,

		/// <summary>
		/// No
		/// </summary>
		n
	}
}