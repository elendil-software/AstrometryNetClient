namespace software.elendil.AstrometryNet.Json
{
	/// <summary>
	/// Result of a <see cref="RequestSender.getObjectsInField">getObjectsInField</see> request
	/// </summary>
	/// 
	public class ObjectsInFieldResponse
	{
		/// <summary>
		/// Object that are in the field
		/// </summary>
		public string[] objects_in_field { get; set; }
	}
}