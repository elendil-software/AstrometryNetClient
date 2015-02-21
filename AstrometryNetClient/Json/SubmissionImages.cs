﻿
namespace software.elendil.AstrometryNetClient.Json
{
	/// <summary>
	/// JSON object sent to get the images of a submission
	/// </summary>
	/// <seealso cref="Client.getSubmissionImages"/>
	public class SubmissionImages
	{
		/// <summary>
		/// Submission ID
		/// </summary>
		public string subid { get; set; }
		/// <summary>
		/// Current active session
		/// </summary>
		public string session { get; set; }
	}
}