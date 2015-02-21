
namespace software.elendil.AstrometryNetClient.Json
{
	/// <summary>
	/// Result of a <see cref="Client.getObjectsInField">getObjectsInField</see> request
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
