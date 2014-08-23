
namespace AstrometryNetClient.Json
{
	/// <summary>
	/// Response of a <see cref="Client.getSubmissionStatus">getSubmissionStatus</see> request
	/// </summary>
	public class SubmissionStatusResponse
	{
		/// <summary>
		/// List of the images in the submission
		/// </summary>
		public string[] user_images { get; set; }
		/// <summary>
		/// List of the jobs in the submission
		/// </summary>
		public string[] jobs { get; set; }
		/// <summary>
		/// User ID
		/// </summary>
		public string user { get; set; }
		/// <summary>
		/// Date and time of the begining of the submission process
		/// </summary>
		public string processing_started { get; set; }
		/// <summary>
		/// Date and time of the end of the submission process
		/// </summary>
		public string processing_finished { get; set; }
	}
}