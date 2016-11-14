using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
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
			const string file = "test.fit";

			try
			{
				var client = new Client(apiKey);
				var res = client.Login();
				Console.WriteLine("Login : " + res.status);
				CancellationTokenSource tokenSource = new CancellationTokenSource();
				CancellationToken token = tokenSource.Token;

				var uploadArguments = new UploadArgs {publicly_visible = Visibility.n};
				var uploadResponse = client.Upload(file, uploadArguments);

				Task<SubmissionStatusResponse> submissionStatusResponse = client.GetSubmissionStatus(uploadResponse.subid, token);
				Task<JobStatusResponse> jobStatusResponse = client.GetJobStatus(submissionStatusResponse.Result.jobs[0], token);

				if (jobStatusResponse.Result.status.Equals(ResponseJobStatus.success))
				{
					var calibrationResponse = client.GetCalibration(submissionStatusResponse.Result.jobs[0]);
					var objectsInFieldResponse = client.GetObjectsInField(submissionStatusResponse.Result.jobs[0]);

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
					Console.WriteLine("Status : " + jobStatusResponse.Result.status);
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