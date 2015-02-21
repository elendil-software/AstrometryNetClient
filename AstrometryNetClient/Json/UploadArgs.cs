using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using software.elendil.AstrometryNetClient.Enum;

namespace software.elendil.AstrometryNetClient.Json
{
	/// <summary>
	/// Parameters to use to upload an image.
	/// 
	/// Only <code>session</code> is required.
	/// If allow_commercial_use or allow_modifications are not defined the default values are used
	/// If publicly_visible is not defined, <code>yes</code> is used
	/// </summary>
	public class UploadArgs
	{
		/// <summary>
		/// Allow commercial use?
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public AllowCommercialUse allow_commercial_use { get; set; }

		/// <summary>
		/// Allow modifications?
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public AllowModifications allow_modifications { get; set; }

		/// <summary>
		/// Show publicly the image on astrometry.net
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public Visibility publicly_visible { get; set; }

		/// <summary>
		/// Scale units. Have to be defined if custom scale is used
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ScaleUnits? scale_units { get; set; }

		/// <summary>
		/// Scale type (bounds or estimate +/- error)
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public ScaleType? scale_type { get; set; }

		/// <summary>
		/// Lower bound. Has to be defined if custom scale is used and scale type is set to "bounds"
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? scale_lower { get; set; }

		/// <summary>
		/// Upper bound. Has to be defined if custom scale is used and scale type is set to "bounds"
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? scale_upper { get; set; }

		/// <summary>
		/// Scale estimation. Has to be defined if custom scale is used and scale type is set to "estimate +/- error"
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? scale_est { get; set; }

		/// <summary>
		/// Scale error estimation. Has to be defined if custom scale is used and scale type is set to "estimate +/- error"
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? scale_err { get; set; }

		/// <summary>
		/// If defined, search only in the specified area. To be used with center_dec and radius 
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? center_ra { get; set; }

		/// <summary>
		/// If defined, search only in the specified area. To be used with center_ra and radius 
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? center_dec { get; set; }

		/// <summary>
		/// If defined, search only in the specified area. To be used with center_ra and center_dec 
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public double? radius { get; set; }

		/// <summary>
		/// Downsample the image by a given factor before performing the source extraction.
		/// </summary>
		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public int? downsample_factor { get; set; }

		/// <summary>
		/// The session you get with Client.Login
		/// </summary>
		public string session { get; set; }
	}
}
