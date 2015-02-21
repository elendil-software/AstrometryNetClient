using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
				var client = new RequestSender(apiKey);
				var res = client.Login();
				Console.WriteLine("Login : " + res.status);

				var arg = new UploadArgs {publicly_visible = Visibility.n};
				var ur = client.Upload(file, arg);

				SubmissionImagesResponse sir = new SubmissionImagesResponse();
				do
				{
					try
					{
						Console.WriteLine("\nWaiting for images submissions...");
						sir = client.GetSubmissionImages(ur.subid);
						Thread.Sleep(1000);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				} while (sir.image_ids.Length == 0);

				var next = false;
				var ssr = new SubmissionStatusResponse();
				string[] jobs = {};

				do
				{
					try
					{
						Console.WriteLine("\nWaiting for jobs...");
						ssr = client.GetSubmissionStatus(ur.subid);
						Thread.Sleep(500);
						jobs = ssr.jobs.Where(x => !string.IsNullOrEmpty(x)).ToArray();
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						next = true;
					}
				} while (jobs.Length == 0 && !next);


				var jsr = new JobStatusResponse();
				do
				{
					try
					{
						Console.WriteLine("\nSolving...");
						jsr = client.GetJobStatus(ssr.jobs[0]);
						Thread.Sleep(1000);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				} while (jsr.status.Equals(ResponseJobStatus.solving));

				if (jsr.status.Equals(ResponseJobStatus.success))
				{
					CalibrationResponse cr = client.GetCalibration(ssr.jobs[0]);
					ObjectsInFieldResponse oifr = client.GetObjectsInField(ssr.jobs[0]);

					Console.WriteLine("\nRA : " + cr.ra);
					Console.WriteLine("Dec : " + cr.dec);
					Console.WriteLine("radius : " + cr.radius);

					Console.WriteLine("");
					foreach (string obj in oifr.objects_in_field)
					{
						Console.WriteLine(obj);
					}
				}
				else
				{
					Console.WriteLine("Status : " + jsr.status);
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