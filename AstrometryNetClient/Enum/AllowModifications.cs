using software.elendil.AstrometryNetClient.Json;

namespace software.elendil.AstrometryNetClient.Enum
{
	/// <summary>
	/// indicates if modifications of your image are allowed. For more details, see http://nova.astrometry.net/upload
	/// </summary>
	/// <seealso cref="UploadArgs.allow_modifications"/>
	public enum AllowModifications
	{
		/// <summary>
		/// Default
		/// </summary>
		d,

		/// <summary>
		/// yes
		/// </summary>
		y,

		/// <summary>
		/// yes, but share alike
		/// </summary>
		sa,

		/// <summary>
		/// no
		/// </summary>
		n
	}
}