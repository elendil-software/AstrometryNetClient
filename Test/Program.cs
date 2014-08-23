using AstrometryNetClient.Json;
using AstrometryNetClient.Enum;
using System;
using System.Threading;
using AstrometryNetClient;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			string apiKey = "astrometrynetapikey";
			//string file = "C:/Users/Julien/Documents/Visual Studio 2012/Projects/imgTest/CCD Image 1.fit";
			string file = "C:/Users/Julien/Documents/Visual Studio 2012/Projects/imgTest/NGC_6188.jpg";

			try
			{
				Client client = new Client(apiKey);
				LoginResponse res = client.Login();

				UploadArgs arg = new UploadArgs { publicly_visible = Visibility.n };
				UploadResponse ur = client.Upload(file, arg);

				SubmissionImagesResponse sir = new SubmissionImagesResponse();
				do
				{
					try
					{
						sir = client.GetSubmissionImages(ur.subid);
						Console.WriteLine("Waiting for images submissions...");
						Thread.Sleep(3000);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
					}
				} while (sir.image_ids.Length == 0);

				bool next = false;
				SubmissionStatusResponse ssr = new SubmissionStatusResponse();
				do
				{
					try
					{
						ssr = client.GetSubmissionStatus(ur.subid);
						Console.WriteLine("Waiting for jobs...");
						Thread.Sleep(1000);
					}
					catch (Exception e)
					{
						Console.WriteLine(e.Message);
						next = true;
					}
				} while (ssr.jobs.Length == 0 && !next);


				JobStatusResponse jsr = new JobStatusResponse();
				do
				{
					try
					{
						jsr = client.GetJobStatus(ssr.jobs[0]);
						Console.WriteLine("Solving...");
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

					Console.WriteLine("RA : " + cr.ra);
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


				Console.ReadKey();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.ReadKey();
			}
		}
	}
}
