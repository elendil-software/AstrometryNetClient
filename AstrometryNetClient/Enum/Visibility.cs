using software.elendil.AstrometryNetClient.Json;

namespace software.elendil.AstrometryNetClient.Enum
{
	/// <summary>
	/// Visibility of your image on astrometry.net
	/// </summary>
	/// <seealso cref="UploadArgs.publicly_visible"/>
	public enum Visibility
	{
		/// <summary>
		/// yes (anyone can see the image)
		/// </summary>
		y,

		/// <summary>
		/// no (the image can be viewed only from the submitter account
		/// </summary>
		n
	}
}