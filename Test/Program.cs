using System;
using System.Diagnostics;
using software.elendil.AstrometryNet;
using software.elendil.AstrometryNet.Enum;
using software.elendil.AstrometryNet.Json;

namespace Test
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var writer = new TextWriterTraceListener(Console.Out);
			Debug.Listeners.Add(writer);

			const string apiKey = "astrometrynetapikey";
			const string file = "D:/Documents/Visual Studio 2013/Projects/AstrometryNetClient/Test/test.fit";

			try
			{
				var client2 = new Client(apiKey);
				var res2 = client2.Login();
				Console.WriteLine("Login : " + res2.status);

				var uploadArguments = new UploadArgs {publicly_visible = Visibility.n};
				var uploadResponse = client2.Upload(file, uploadArguments);

				//SubmissionImagesResponse submissionImagesResponse = client2.GetSubmissionImages(uploadResponse.subid);
				SubmissionStatusResponse submissionStatusResponse = client2.GetSubmissionStatus(uploadResponse.subid);
				JobStatusResponse jobStatusResponse = client2.GetJobStatus(submissionStatusResponse.jobs[0]);

				if (jobStatusResponse.status.Equals(ResponseJobStatus.success))
				{
					var calibrationResponse = client2.GetCalibration(submissionStatusResponse.jobs[0]);
					var objectsInFieldResponse = client2.GetObjectsInField(submissionStatusResponse.jobs[0]);

					Console.WriteLine("\nRA : " + calibrationResponse.ra);
					Console.WriteLine("Dec : " + calibrationResponse.dec);
					Console.WriteLine("radius : " + calibrationResponse.radius);

					Console.WriteLine("");
					foreach (string obj in objectsInFieldResponse.objects_in_field)
					{
						Console.WriteLine(obj);
					}
				}
				else
				{
					Console.WriteLine("Status : " + jobStatusResponse.status);
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				Console.ReadKey();
			}
		}
	}
}