﻿using software.elendil.AstrometryNet.Json;

namespace software.elendil.AstrometryNet.Enum
{
	/// <summary>
	/// Define the scale type if custom scale is used
	/// </summary>
	/// <seealso cref="UploadArgs.scale_type"/>
	public enum ScaleType
	{
		/// <summary>
		/// bounds
		/// </summary>
		ul,

		/// <summary>
		/// estimate +/- error
		/// </summary>
		ev
	}
}