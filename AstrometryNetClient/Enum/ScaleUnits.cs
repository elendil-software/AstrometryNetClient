using software.elendil.AstrometryNetClient.Json;

namespace software.elendil.AstrometryNetClient.Enum
{
	/// <summary>
	/// Defines the units when custom scale is used
	/// </summary>
	/// <seealso cref="UploadArgs.scale_units"/>
	public enum ScaleUnits
	{
		/// <summary>
		/// arcseconds per pixel
		/// </summary>
		arcsecperpix,

		/// <summary>
		/// width of the field (in arcminutes)
		/// </summary>
		arcminwidth,

		/// <summary>
		/// width of the field (in degrees)
		/// </summary>
		degwidth,

		/// <summary>
		/// focal length of the lens (for 35mm film equivalent sensor)
		/// </summary>
		focalmm
	}
}